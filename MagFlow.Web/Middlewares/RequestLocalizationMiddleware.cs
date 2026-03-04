using MagFlow.BLL.Mappers.Enum;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.DAL.Repositories.CoreScope.Interfaces;
using MagFlow.Domain.CoreScope;
using MagFlow.Shared.DTOs.CoreScope;
using MagFlow.Shared.Models;
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
            UserSettingsDTO? userSettings = null;
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
                userSettings = user?.Settings;
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

            if (userSettings == null)
            {
                culture.NumberFormat.NumberDecimalSeparator = ".";
                culture.DateTimeFormat.LongDatePattern = "dddd, dd MMMM yyyy";
                culture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
                culture.DateTimeFormat.FullDateTimePattern = "yyyy-MM-dd HH:mm:ss";
                culture.DateTimeFormat.LongTimePattern = "HH:mm:ss";
                culture.DateTimeFormat.ShortTimePattern = "HH:mm";
            }
            else
            {
                culture.NumberFormat.NumberDecimalSeparator = userSettings.DecimalSeparator == Enums.DecimalSeparator.Dot 
                    ? "." : userSettings.DecimalSeparator == Enums.DecimalSeparator.Comma 
                    ? "," : ".";
                culture.DateTimeFormat.LongDatePattern = "dddd, dd MMMM yyyy";
                var shortDatePattern = userSettings.DateFormat == Enums.DateFormat.RRRR_MM_DD_DASHES 
                    ? "yyyy-MM-dd" : userSettings.DateFormat == Enums.DateFormat.DD_MM_RRRR_DASHES
                    ? "dd-MM-yyyy" : userSettings.DateFormat == Enums.DateFormat.DD_MM_RRRR_DOTS 
                    ? "dd.MM.yyyy" : userSettings.DateFormat == Enums.DateFormat.MM_DD_RRRR_SLASHES
                    ? "MM'/'dd'/'yyyy" : "yyyy-MM-dd";
                culture.DateTimeFormat.ShortDatePattern = shortDatePattern;
                var longTimePattern = userSettings.TimeFormat == Enums.TimeFormat.HH_MM_24H
                    ? "HH:mm:ss" : userSettings.TimeFormat == Enums.TimeFormat.HH_MM_12H
                    ? "hh:mm:ss tt" : "HH:mm:ss";
                var shortTimePattern = userSettings.TimeFormat == Enums.TimeFormat.HH_MM_24H
                    ? "HH:mm" : userSettings.TimeFormat == Enums.TimeFormat.HH_MM_12H
                    ? "hh:mm tt" : "HH:mm";
                culture.DateTimeFormat.LongTimePattern = longTimePattern;
                culture.DateTimeFormat.ShortTimePattern = shortTimePattern;
                culture.DateTimeFormat.FullDateTimePattern = $"{shortDatePattern} {longTimePattern}";
            }
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            await _next(context);
        }
    }
}
