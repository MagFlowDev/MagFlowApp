using MagFlow.Shared.Models;

namespace MagFlow.Web.Pages.Modules.Contractors
{
    public partial class Contractors : BaseModuleComponent
    {
        protected override Enum _currentSection { get; set; } = SectionsEnums.ContractorsModuleSection.ContractorsList;

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
