using MagFlow.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace MagFlow.Web.Pages.Modules.Wares
{
    public partial class Wares : BaseModuleComponent
    {
        protected override Enum _currentSection { get; set; } = SectionsEnums.WaresModuleSection.WaresList;

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
