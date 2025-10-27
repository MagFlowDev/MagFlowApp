using MagFlow.Shared.Models.Settings;

namespace MagFlow.Web.Helpers
{
    public static class ConfigurationMapper
    {
        public static void MapToAppSettings(this ConfigurationManager manager)
        {
            AppSettings.ConnectionStrings = manager.GetSection(nameof(AppSettings.ConnectionStrings)).Get<ConnectionStrings>() ?? throw new Exception($"{nameof(AppSettings.ConnectionStrings)} not found in appsettings.json");
        }
    }
}
