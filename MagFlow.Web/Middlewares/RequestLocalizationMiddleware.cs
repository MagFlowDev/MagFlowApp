using MagFlow.BLL.Mappers.Enum;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.DAL.Repositories.Core.Interfaces;
using MagFlow.Domain.Core;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using System.Security.Claims;

namespace MagFlow.Web.Middlewares
{
    public class RequestLocalizationMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLocalizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUserService userService)
        {
            bool userLanguageSelected = false;
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(!string.IsNullOrEmpty(userId))
            {
                Guid.TryParse(userId, out Guid userGuid);
                var user = await userService.GetUser(userGuid);
                var lang = user?.Settings?.Language;
                if(lang != null)
                {
                    var language = lang?.ToLanguageCode()!;
                    var culture = new CultureInfo(language);

                    CultureInfo.DefaultThreadCurrentCulture = culture;
                    CultureInfo.DefaultThreadCurrentUICulture = culture;
                    userLanguageSelected = true;
                }
            }

            if (!userLanguageSelected)
            {
                var userLanguages = context.Request.Headers["Accept-Language"].ToString();
                if (!string.IsNullOrWhiteSpace(userLanguages))
                {
                    var preferredLanguage = userLanguages.Split(',')[0];
                    var language = LanguageMapper.GetAvailableLanguageByCode(preferredLanguage);
                    var culture = new CultureInfo(language);

                    CultureInfo.DefaultThreadCurrentCulture = culture;
                    CultureInfo.DefaultThreadCurrentUICulture = culture;
                }
            }

            await _next(context);
        }
    }
}
