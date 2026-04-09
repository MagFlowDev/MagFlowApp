using MagFlow.Shared.Models;
using MudBlazor;

namespace MagFlow.Web.Pages.Modules.Users
{
    public partial class Users : BaseModuleComponent
    {
        protected override async Task OnInitializedAsync()
        {

        }

        private void OnSectionChanged(SectionsEnums.UserModuleSection section)
        {
            if (_currentSection == section)
                return;
            _currentSection = section;
            StateHasChanged();
        }

        private string GetNavClass(SectionsEnums.UserModuleSection section)
        {
            return _currentSection == section
                ? "mud-primary-text mud-nav-link-active"
                : "";
        }

        private Color GetIconColor(SectionsEnums.UserModuleSection section)
        {
            return _currentSection == section
                ? Color.Primary
                : Color.Dark;
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
