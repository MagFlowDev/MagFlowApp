using MagFlow.Shared.Constants;
using Microsoft.Extensions.Localization;

namespace MagFlow.BLL.Helpers.Localization.Display
{
    public static class HistoryEventDisplayHelper
    {
        public static string DisplayHistoryEvent(this string? eventType, IStringLocalizer localizer)
        {
            return eventType switch
            {
                HistoryEventTypes.ENTITY_CREATED => localizer["HistoryEntityCreated"].Value,
                HistoryEventTypes.ENTITY_UPDATED => localizer["HistoryEntityUpdated"].Value,
                _ => eventType ?? string.Empty
            };
        }
    }
}
