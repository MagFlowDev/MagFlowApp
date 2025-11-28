using MagFlow.BLL.Mappers.Enum;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.DAL.Repositories.Core.Interfaces;
using MagFlow.Domain.Core;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using System.Security.Claims;
using System.Text.RegularExpressions;

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
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            CultureInfo? culture = null;
            if(!string.IsNullOrEmpty(userId))
            {
                Guid.TryParse(userId, out Guid userGuid);
                var user = await userService.GetUser(userGuid);
                var lang = user?.Settings?.Language;
                if(lang != null)
                {
                    var language = lang?.ToLanguageCode()!;
                    culture = new CultureInfo(language);
                }
            }

            if (culture == null)
            {
                var userLanguages = context.Request.Headers["Accept-Language"].ToString();
                if (!string.IsNullOrWhiteSpace(userLanguages))
                {
                    var preferredLanguage = userLanguages.Split(',')[0];
                    var language = LanguageMapper.GetAvailableLanguageByCode(preferredLanguage);
                    culture = new CultureInfo(language);
                    culture.NumberFormat.NumberDecimalSeparator = ".";
                }
                else
                {
                    culture = new CultureInfo("pl-PL");
                }
            }

            culture.NumberFormat.NumberDecimalSeparator = ".";
            culture.DateTimeFormat.LongDatePattern = "yyyy-MM-dd HH:mm";
            culture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            await _next(context);
        }
    }
}
