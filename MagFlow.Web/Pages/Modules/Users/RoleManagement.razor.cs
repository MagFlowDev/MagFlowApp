using Google.Protobuf.WellKnownTypes;
using MagFlow.BLL.Services;
using MagFlow.Shared.DTOs.CoreScope;
using MagFlow.Shared.Models.Enumerators;

namespace MagFlow.Web.Pages.Modules.Users
{
    public partial class RoleManagement : AuthComponentBase
    {
        List<ModuleDTO>? _modules = null;
        Dictionary<Guid, Dictionary<Guid, RoleModuleAccess>> _accessList = [];
        Dictionary<Guid, RoleModuleAccess> _roleAccessList = [];

        private bool _isBusy;
        private bool _loading;

        private AppRole _selectedRole
        {
            get => field;
            set
            {
                field = value;
                OnSelectedRoleChanged(value);
            }
        }

        private void OnSelectedRoleChanged(AppRole value)
        {
            if (_accessList.TryGetValue(value.Id, out var accessList))
                _roleAccessList = accessList;
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _selectedRole = AppRole.Foreman;
            if(_currentCompanyId.HasValue)
            {
                _modules = await CompanyService.GetCompanyModules(_currentCompanyId.Value);
            }
            await SetupAccessList();
        }

        private async Task SetupAccessList()
        {
            _accessList = new Dictionary<Guid, Dictionary<Guid, RoleModuleAccess>>();
            foreach(var appRole in Enumeration<Guid>.GetAll<AppRole>())
            {
                var roleAccessList = new Dictionary<Guid, RoleModuleAccess>();
                foreach(var module in _modules ?? new List<ModuleDTO>())
                {
                    roleAccessList.TryAdd(module.Id, new RoleModuleAccess()
                    {
                         
                    });
                }
                _accessList.TryAdd(appRole.Id, roleAccessList);
            }
            if (_accessList.TryGetValue(_selectedRole.Id, out var accessList))
                _roleAccessList = accessList;
        }

        private async Task Save()
        {
            if (_loading || _isBusy)
                return;

            try
            {
                _isBusy = true;
                _loading = true;

                await Task.Delay(2000);
            }
            finally
            {
                _isBusy = false;
                _loading = false;
            }
        }

        private class RoleModuleAccess
        {
            public bool Read { get; set; }
            public bool Add { get; set; }
            public bool Edit { get; set; }
            public bool Delete { get; set; }
            public bool Admin { get; set; }
        }
    }
}
