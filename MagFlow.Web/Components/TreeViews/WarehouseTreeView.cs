using MudBlazor;

namespace MagFlow.Web.Components.TreeViews
{
    public class WarehouseTreeView
    {
    }

    public class WarehouseTreeViewTempItem : TreeItemData<string>
    {
        public int SubItemsCount => this.Children?.Count ?? 0;

        public WarehouseTreeViewTempItem(string text) : base(text)
        {
            Text = text;
        }
    }

    public class WarehouseTreeViewItem : TreeItemData<int>
    {
        public int SubItemsCount => this.Children?.Count ?? 0;

        public WarehouseTreeViewItem(int id, string text) : base(id)
        {
            Text = text;
        }
    }
}
