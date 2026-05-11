using MagFlow.Shared.Models;

namespace MagFlow.Web.Pages.Modules.Wares
{
    public partial class Wares : BaseModuleComponent
    {
        private bool _isMenuOpened = false;
        private SectionsEnums.WaresModuleSection _currentSection = SectionsEnums.WaresModuleSection.WaresList;

        protected override async Task OnInitializedAsync()
        {

        }

        private void OnSectionChanged(SectionsEnums.WaresModuleSection section)
        {
            if (_currentSection == section)
                return;
            _currentSection = section;
            StateHasChanged();
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
