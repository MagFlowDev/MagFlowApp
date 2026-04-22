using Google.Protobuf.WellKnownTypes;
using MagFlow.BLL.Services;
using MagFlow.Shared.DTOs.CoreScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Enumerators;
using MagFlow.Web.Resources;
using MudBlazor;

namespace MagFlow.Web.Pages.Modules.Users
{
    public partial class RoleManagement : AuthComponentBase
    {
        List<ModuleDTO>? _modules = null;
        Dictionary<Guid, Dictionary<Guid, RoleModuleAccess>> _accessList = [];
        Dictionary<Guid, RoleModuleAccess> _roleAccessList = [];
        Dictionary<Guid, List<ClaimDTO>> _rolesClaims = [];
        List<ClaimDTO> _allClaims = [];

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
            var allRoles = Enumeration<Guid>.GetAll<AppRole>().ToList();
            _rolesClaims = await RoleService.GetRolesClaims(allRoles.Select(x => x.Id).ToList());
            _allClaims = await RoleService.GetAllClaims();

            _accessList = new Dictionary<Guid, Dictionary<Guid, RoleModuleAccess>>();
            foreach(var appRole in allRoles)
            {
                var roleAccessList = new Dictionary<Guid, RoleModuleAccess>();
                if(_rolesClaims.TryGetValue(appRole.Id, out var claims) && claims.Count > 0)
                {
                    foreach(var claim in claims)
                    {
                        var claimNameParts = claim.Policy.Split('.');
                        if(claimNameParts.Length == 2)
                        {
                            var moduleId = _modules?.FirstOrDefault(x => x.Name.Contains(claimNameParts[0], StringComparison.OrdinalIgnoreCase))?.Id;
                            if (!moduleId.HasValue)
                                continue;
                            var accessType = claimNameParts[1];
                            if(!roleAccessList.TryGetValue(moduleId.Value, out var roleModuleAccess))
                            {
                                roleModuleAccess = new RoleModuleAccess();
                                roleModuleAccess.ModuleName = claimNameParts[0];
                                roleAccessList.Add(moduleId.Value, roleModuleAccess);
                            }
                            switch(accessType)
                            {
                                case "Read":
                                    roleModuleAccess.Read = true;
                                    break;
                                case "Add":
                                    roleModuleAccess.Add = true;
                                    break;
                                case "Edit":
                                    roleModuleAccess.Edit = true;
                                    break;
                                case "Delete":
                                    roleModuleAccess.Delete = true;
                                    break;
                                case "Admin":
                                    roleModuleAccess.Admin = true;
                                    break;
                            }
                        }
                    }
                }
                // Add any modules that don't have claims for this role with default access
                foreach (var module in _modules ?? new List<ModuleDTO>())
                {
                    if(roleAccessList.ContainsKey(module.Id))
                        continue;
                    var moduleName = module.Name
                        .Replace("Module", "")
                        .Replace("module", "")
                        .Replace(".","")
                        .Replace(",","")
                        .Replace("_","")
                        .Replace(":","");
                    roleAccessList.TryAdd(module.Id, new RoleModuleAccess() { ModuleName = moduleName });
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

                foreach(var roleAccessList in _accessList)
                {
                    var roleId = roleAccessList.Key;
                    List<ClaimDTO> roleClaims;
                    if(_rolesClaims.TryGetValue(roleId, out var claims))
                        roleClaims = claims;
                    else
                    {
                        roleClaims = [];
                        _rolesClaims.TryAdd(roleId, roleClaims);
                    }
                    
                    foreach(var moduleAccessList in roleAccessList.Value)
                    {
                        var moduleId = moduleAccessList.Key;
                        var module = moduleAccessList.Value.ModuleName;
                        
                        var claimTypes = new List<string> { "Read", "Add", "Edit", "Delete", "Admin" };
                        foreach(var claimType in claimTypes)
                        {
                            var claim = roleClaims.FirstOrDefault(x => x.Policy == $"{module}.{claimType}");
                            var moduleAccessProperty = moduleAccessList.Value.GetType().GetProperty(claimType)?.GetValue(moduleAccessList.Value);
                            if(moduleAccessProperty != null && moduleAccessProperty is bool hasAccess)
                            {
                                if(hasAccess && claim == null)
                                {
                                    claim = _allClaims.FirstOrDefault(x => x.Policy == $"{module}.{claimType}");
                                    if(claim == null)
                                        claim = new ClaimDTO { Name = $"{module.ToUpper()}_{claimType.ToUpper()}", Policy = $"{module}.{claimType}" };
                                    claim.ToAdd = true;
                                    roleClaims.Add(claim);
                                }
                                else if(!hasAccess && claim != null)
                                    claim.ToDelete = true;
                            }
                        }
                    }
                }

                var result = await RoleService.UpdateRolesClaims(_rolesClaims);
                if (result == Enums.Result.Success)
                {
                    var allRoles = Enumeration<Guid>.GetAll<AppRole>().ToList();
                    _rolesClaims = await RoleService.GetRolesClaims(allRoles.Select(x => x.Id).ToList());
                    Snackbar.Add(Localizer[Langs.SettingsSaved], Severity.Success);
                }
                else
                {
                    Snackbar.Add(Localizer[Langs.ErrorOccured], Severity.Error);
                }
            }
            finally
            {
                _isBusy = false;
                _loading = false;
            }
        }

        private class RoleModuleAccess
        {
            public string ModuleName { get; set; }

            public bool Read { get; set; }
            public bool Add { get; set; }
            public bool Edit { get; set; }
            public bool Delete { get; set; }
            public bool Admin { get; set; }
        }
    }
}
