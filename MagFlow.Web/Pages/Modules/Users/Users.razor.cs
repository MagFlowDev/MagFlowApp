using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Enumerators;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MagFlow.Web.Pages.Modules.Users
{
    public partial class Users : BaseModuleComponent
    {
        protected override Enum _currentSection { get; set; } = SectionsEnums.UsersModuleSection.UsersList;

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
