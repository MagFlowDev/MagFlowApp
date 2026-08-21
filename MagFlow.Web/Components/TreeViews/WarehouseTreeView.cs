using MagFlow.Shared.Models;
using MudBlazor;

namespace MagFlow.Web.Components.TreeViews
{
    public class WarehouseTreeView
    {
    }

    public class WarehouseTreeViewTempItem : TreeItemData<string>
    {
        public int SubItemsCount => this.Children?.Count ?? 0;

        public Enums.WarehouseStorageType StorageType { get; set; }

        public Guid TempId { get; set; }

        public Guid? ParentId { get; set; }

        public WarehouseTreeViewTempItem(string text, Enums.WarehouseStorageType storageType, Guid? parentId = null) : base(text)
        {
            TempId = Guid.NewGuid();
            Text = text;
            StorageType = storageType;
            ParentId = parentId;
        }

        public void AddChildren(WarehouseTreeViewTempItem child)
        {
            var currentChildren = this.Children != null 
                ? this.Children.Cast<ITreeItemData<string>>().ToList()
                : new List<ITreeItemData<string>>();

            currentChildren.Add(child);

            this.Children = currentChildren;
        }

        public void RemoveChildren(WarehouseTreeViewTempItem child)
        {
            var currentChildren = this.Children != null
                ? this.Children.Cast<ITreeItemData<string>>().ToList()
                : new List<ITreeItemData<string>>();

            var existingChild = currentChildren.FirstOrDefault(x => ((WarehouseTreeViewTempItem)x) != null && ((WarehouseTreeViewTempItem)x).TempId == child.TempId);
            if (existingChild == null)
                return;

            currentChildren.Remove(existingChild);

            this.Children = currentChildren;
        }
    }

    public class WarehouseTreeViewItem : TreeItemData<int>
    {
        public int SubItemsCount => this.Children?.Count ?? 0;

        public WarehouseTreeViewItem(int id, string text) : base(id)
        {
            Text = text;
        }

        public void AddChildren(WarehouseTreeViewItem child)
        {
            var currentChildren = this.Children != null
                ? this.Children.Cast<ITreeItemData<int>>().ToList()
                : new List<ITreeItemData<int>>();

            currentChildren.Add(child);

            this.Children = currentChildren;
        }
    }
}
