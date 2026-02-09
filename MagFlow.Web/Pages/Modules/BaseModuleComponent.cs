using Microsoft.AspNetCore.Components;

namespace MagFlow.Web.Pages.Modules
{
    public abstract class BaseModuleComponent : ComponentBase, IDisposable
    {
        private CancellationTokenSource _cts = new();

        protected CancellationToken CancellationToken => _cts.Token;

        public virtual void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}
