using MagFlow.Shared.Models.Settings;

namespace MagFlow.Web.Helpers
{
    public static class ConfigurationMapper
    {
        public static void MapToAppSettings(this ConfigurationManager manager)
        {
            AppSettings.ConnectionStrings = manager.GetSection(nameof(AppSettings.ConnectionStrings)).Get<ConnectionStrings>() ?? throw new Exception($"{nameof(AppSettings.ConnectionStrings)} not found in appsettings.json");
            AppSettings.OtelSettings = manager.GetSection(nameof(AppSettings.OtelSettings)).Get<OtelSettings>() ?? throw new Exception($"{nameof(AppSettings.OtelSettings)} not found in appsettings.json");
            AppSettings.SmtpSettings = manager.GetSection(nameof(AppSettings.SmtpSettings)).Get<SmtpSettings>() ?? throw new Exception($"{nameof(AppSettings.SmtpSettings)} not found in appsettings.json");
        }
    }
}
