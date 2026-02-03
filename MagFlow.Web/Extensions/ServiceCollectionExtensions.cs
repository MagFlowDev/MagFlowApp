using Castle.DynamicProxy;
using FormCraft;
using FormCraft.ForMudBlazor.Extensions;
using MagFlow.BLL.ApplicationMonitor;
using MagFlow.BLL.Helpers;
using MagFlow.BLL.Helpers.Auth;
using MagFlow.BLL.Hubs;
using MagFlow.BLL.Services;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.BLL.Services.Notifications;
using MagFlow.DAL.Repositories.Company;
using MagFlow.DAL.Repositories.Company.Interfaces;
using MagFlow.DAL.Repositories.Core;
using MagFlow.DAL.Repositories.Core.Interfaces;
using MagFlow.Domain.Core;
using MagFlow.EF;
using MagFlow.EF.MultiTenancy;
using MagFlow.Shared.Models.Settings;
using MagFlow.Web.HealthChecks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MudBlazor;
using MudBlazor.Services;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace MagFlow.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMagFlowServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSignalR();
            services.AddMudServices(config =>
            {
                config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
                config.SnackbarConfiguration.MaxDisplayedSnackbars = 5;
                config.SnackbarConfiguration.PreventDuplicates = true;
                config.SnackbarConfiguration.NewestOnTop = true;
                config.SnackbarConfiguration.ShowCloseIcon = true;
                config.SnackbarConfiguration.VisibleStateDuration = 7000;
                config.SnackbarConfiguration.HideTransitionDuration = 500;
                config.SnackbarConfiguration.ShowTransitionDuration = 500;
                config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
            });
            services.AddRazorComponents()
                .AddInteractiveServerComponents();

            services.AddFormCraft();
            services.AddFormCraftMudBlazor();

            services.RegisterScopes();
            services.ConfigureDbContext();
            services.ConfigureAuthorization();

            services.AddHttpClient();
            services.AddScoped(sp =>
            {
                var navigationManager = sp.GetRequiredService<NavigationManager>();
                var handler = new HttpClientHandler
                {
                    UseCookies = true,
                    CookieContainer = new System.Net.CookieContainer()
                };
                return new HttpClient(handler)
                {
                    BaseAddress = new Uri(navigationManager.BaseUri)
                };
            });
            services.AddSingleton<IServerNotificationService, ServerNotificationsService>();
            services.AddSingleton<IUserIdProvider, HubUserIdProvider>();

            services.ConfigureOpenTelemetry();
            services.AddHttpContextAccessor();
            services.AddLocalization();

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor |
                    Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto;

                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });

            services.AddHostedService<ApplicationMonitorHostedService>();

            services.AddMagFlowHealthChecks();
            return services;
        }

        private static IServiceCollection AddMagFlowHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy(), tags: new[] { "live" })
                .AddCheck<SqlServerPingHealthCheck>("sql", tags: new[] { "ready" });

            return services;
        }

        private static void ConfigureDbContext(this IServiceCollection services)
        {
            services.AddScoped<ITenantProvider, TenantProvider>();
            services.AddScoped<ICompanyContext, CompanyContext>();
            services.AddScoped<ICoreDbContextFactory, CoreDbContextFactory>();
            services.AddScoped<ICompanyDbContextFactory, CompanyDbContextFactory>();
            services.AddDbContextFactory<CoreDbContext, CoreDbContextFactory>();
        }

        private static void ConfigureAuthorization(this IServiceCollection services)
        {
            services.AddCascadingAuthenticationState();
            services.AddScoped<IdentityUserAccessor>();
            services.AddScoped<IdentityRedirectManager>();
            services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
            services.AddScoped<UserManager<ApplicationUser>>();
            services.AddScoped<RoleManager<ApplicationRole>>();
            services.AddScoped<SignInManager<ApplicationUser>>();
            services.AddScoped<IEmailSender<ApplicationUser>>(sp => sp.GetRequiredService<IEmailService>());
            services.AddScoped<IUserStore<ApplicationUser>, FactoryUserStore>();
            services.AddScoped<IRoleStore<ApplicationRole>, FactoryRoleStore>();

            services.AddAuthentication(o =>
                {
                    o.DefaultScheme = IdentityConstants.ApplicationScheme;
                    o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                })
                .AddIdentityCookies();
            services.AddIdentityCore<ApplicationUser>(options =>
                {
                    options.Stores.MaxLengthForKeys = 128;
                    options.SignIn.RequireConfirmedEmail = true;
                    options.Lockout = new LockoutOptions()
                    {
                        MaxFailedAccessAttempts = 10,
                        DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1)
                    };
                })
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<CoreDbContext>()
                .AddUserManager<UserManager<ApplicationUser>>()
                .AddRoleManager<RoleManager<ApplicationRole>>()
                .AddSignInManager()
                .AddDefaultTokenProviders();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Login";
                options.LogoutPath = "/Logout";
                options.AccessDeniedPath = "/Denied";

                options.Cookie.MaxAge = TimeSpan.FromHours(12);
                options.SlidingExpiration = true;

                options.Cookie.Name = "MagFlow.Auth";
                options.Cookie.Path = "/";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.Cookie.SameSite = SameSiteMode.Lax;
            });
            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.FromMinutes(30);
            });
        }

        private static void RegisterScopes(this IServiceCollection services)
        {
            services.RegisterRepositories();
            // services.RegisterServices();
            // Use instead of standard services.RegisterServices()
            // It uses SecurityInterceptor as middleware between blazor pages code and BLL services
            // It checks user permissions to functions/methods with attribute MinimumRole/MustHaveRole
            services.RegisterServicesWithProxy();
        }

        private static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<ICompanyService, CompanyService>();

            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<ClientNotificationService>();
        }

        private static void RegisterServicesWithProxy(this IServiceCollection services)
        {
            services.AddScoped<SecurityInterceptor>();

            services.AddScoped<UserService>();
            services.AddScoped<EventService>();
            services.AddScoped<CompanyService>();

            services.AddScoped<IUserService>(sp => sp.GetRequiredService<UserService>().WithProxy<IUserService>(sp));
            services.AddScoped<IEventService>(sp => sp.GetRequiredService<EventService>().WithProxy<IEventService>(sp));
            services.AddScoped<ICompanyService>(sp => sp.GetRequiredService<CompanyService>().WithProxy<ICompanyService>(sp));

            services.AddScoped<INetworkService, NetworkService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<ClientNotificationService>();
        }

        private static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IEventLogRepository, EventLogRepository>();
            services.AddScoped<ISessionRepository, SessionRepository>();

            services.AddScoped<IContractorRepository, ContractorRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<ILocalUserRepository, LocalUserRepository>();
            services.AddScoped<IMachineRepository, MachineRepository>();
            services.AddScoped<IOrderRepository,  OrderRepository>();
            services.AddScoped<IProcessRepository, ProcessRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IUnitRepository, UnitRepository>();
            services.AddScoped<IWarehouseRepository, WarehouseRepository>();

            services.AddScoped<INotificationRepository, NotificationRepository>();
        }

        private static void ConfigureOpenTelemetry(this IServiceCollection services)
        {
            if (AppSettings.OtelSettings == null || !AppSettings.OtelSettings.Enabled || string.IsNullOrEmpty(AppSettings.OtelSettings.Address))
                return;
            services.AddOpenTelemetry()
               .WithTracing(tracing =>
               {
                   tracing
                       .AddSource("MagFlowActivitySource")
                       .AddAspNetCoreInstrumentation()
                       .AddHttpClientInstrumentation()
                       .SetResourceBuilder(ResourceBuilder.CreateDefault()
                           .AddService("MagFlow")
                           .AddTelemetrySdk())
                       .AddOtlpExporter(options => options.Endpoint = new Uri(AppSettings.OtelSettings.Address));
               })
               .WithMetrics(metrics =>
               {
                   metrics
                       .AddMeter("MagFlowMetrics")
                       .AddAspNetCoreInstrumentation()
                       .AddHttpClientInstrumentation()
                       .SetResourceBuilder(ResourceBuilder.CreateDefault()
                           .AddService("MagFlow")
                           .AddTelemetrySdk())
                       .AddOtlpExporter(options => options.Endpoint = new Uri(AppSettings.OtelSettings.Address));
               });
        }

        private static TInterface WithProxy<TInterface>(this TInterface target, IServiceProvider sp) where TInterface : class
        {
            var proxyGenerator = new ProxyGenerator();
            var interceptor = sp.GetRequiredService<SecurityInterceptor>();

            return proxyGenerator.CreateInterfaceProxyWithTarget<TInterface>(
                target,
                interceptor.ToInterceptor());
        }
    }
}
