using MagFlow.Shared.Models;

namespace MagFlow.Web.Pages.Modules.Wares.Definition
{
    public partial class Definitions
    {
        private SectionsEnums.WaresDefinitionSection _currentSection = SectionsEnums.WaresDefinitionSection.Types;

        private void OnSectionChanged(SectionsEnums.WaresDefinitionSection section)
        {
            if (_currentSection == section)
                return;
            _currentSection = section;
            StateHasChanged();
        }
    }
}
