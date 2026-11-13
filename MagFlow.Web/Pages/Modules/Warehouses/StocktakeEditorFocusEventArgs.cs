using Microsoft.AspNetCore.Components;

namespace MagFlow.Web.Pages.Modules.Warehouses;

[EventHandler("onstocktakeeditorfocusout", typeof(StocktakeEditorFocusEventArgs))]
public static class EventHandlers
{
}

public sealed class StocktakeEditorFocusEventArgs : EventArgs
{
    public bool StaysInsideEditor { get; set; }
}
