using MagFlow.Shared.Models;
using MudBlazor;

namespace MagFlow.Web.Helpers
{
    internal static class StocktakeStatusColors
    {
        public static Color Get(Enums.EntityStatus status) => status switch
        {
            Enums.EntityStatus.InProgress => Color.Info,
            Enums.EntityStatus.AwaitingApproval => Color.Warning,
            Enums.EntityStatus.Confirmed => Color.Success,
            Enums.EntityStatus.Canceled => Color.Error,
            _ => Color.Default
        };
    }
}
