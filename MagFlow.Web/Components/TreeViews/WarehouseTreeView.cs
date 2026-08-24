using MagFlow.Shared.DTOs.CompanyScope;
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

        public SectorDTO? GetSectorDTO()
        {
            if (this.StorageType != Enums.WarehouseStorageType.Sector)
                return null;

            var currentChildren = this.Children != null
                ? this.Children.Cast<WarehouseTreeViewTempItem>().ToList()
                : new List<WarehouseTreeViewTempItem>();

            var rows = currentChildren?
                .Where(x => x != null).Select(x => x!.GetRowDTO())
                .Where(x => x != null).Select(x => x!)
                .ToList() ?? new List<RowDTO>();

            return new SectorDTO()
            {
                Name = Text,
                Rows = rows
            };
        }

        public RowDTO? GetRowDTO()
        {
            if (this.StorageType != Enums.WarehouseStorageType.Row)
                return null;

            var currentChildren = this.Children != null
                ? this.Children.Cast<WarehouseTreeViewTempItem>().ToList()
                : new List<WarehouseTreeViewTempItem>();

            var slots = currentChildren?
                .Where(x => x != null).Select(x => x!.GetSlotDTO())
                .Where(x => x != null).Select(x => x!)
                .ToList() ?? new List<SlotDTO>();

            return new RowDTO()
            {
                Name = Text,
                Slots = slots
            };
        }

        public SlotDTO? GetSlotDTO()
        {
            if (this.StorageType != Enums.WarehouseStorageType.Slot)
                return null;

            return new SlotDTO()
            {
                Name = Text,
            };
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
