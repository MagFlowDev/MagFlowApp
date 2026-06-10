using MagFlow.BLL.Services.Interfaces;
using MagFlow.Domain.CoreScope;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace MagFlow.Web.Pages.Modules
{
    public abstract class BaseModuleComponent : AuthComponentBase, IDisposable
    {
        [CascadingParameter(Name = "SessionId")] 
        protected Guid SessionId { get; set; }
        [CascadingParameter(Name = "ModuleId")] 
        protected Guid ModuleId { get; set; }

        private CancellationTokenSource _cts = new();
        protected virtual Enum _currentSection { get; set; } = BaseSectionEnum.DefaultSection.None;

        protected CancellationToken CancellationToken => _cts.Token;


        [Inject]
        protected NavigationManager NavigationManager { get; set; }
        [Inject]
        protected ILocalCacheService LocalCacheService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            NavigationManager.LocationChanged += OnLocationChanged;
            await ReadSection();
        }

        public virtual void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            NavigationManager?.LocationChanged -= OnLocationChanged;
            SaveSection();
        }

        protected void OnLocationChanged(object? sender, LocationChangedEventArgs e)
        {
            SaveSection();
        }

        protected virtual async Task SaveSection()
        {
            await LocalCacheService.SetCurrentModuleSection(SessionId, ModuleId, _currentSection);
        }

        protected virtual async Task ReadSection()
        {
            var section = await LocalCacheService.GetCurrentModuleSection(SessionId, ModuleId);
            if (section != null)
                _currentSection = section;
        }

        protected virtual void OnSectionChanged(Enum section)
        {
            if (Equals(_currentSection, section))
                return;

            _currentSection = section;
            StateHasChanged();
        }
    }

    public static class BaseSectionEnum
    {
        public enum DefaultSection
        {
            None = 0
        }
    }
}
