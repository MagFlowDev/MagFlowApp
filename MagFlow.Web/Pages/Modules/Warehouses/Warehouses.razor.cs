using MagFlow.Shared.Models;

namespace MagFlow.Web.Pages.Modules.Warehouses
{
    public partial class Warehouses : BaseModuleComponent
    {
        protected override Enum _currentSection { get; set; } = SectionsEnums.WarehousesModuleSection.WarehousesList;

        private bool _isMenuOpened = false;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
