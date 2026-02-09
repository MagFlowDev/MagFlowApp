namespace MagFlow.Web.Pages.Modules.Worktime
{
    public partial class Worktime : BaseModuleComponent
    {
        private int _value = 0;

        protected override async Task OnInitializedAsync()
        {
            SimulateProgressAsync();
        }

        public async Task SimulateProgressAsync()
        {
            try
            {
                _value = 0;
                while (!CancellationToken.IsCancellationRequested)
                {
                    _value += 4;
                    if (_value > 100)
                        _value = 0;
                    StateHasChanged();
                    await Task.Delay(500, CancellationToken);
                }
            }
            catch (OperationCanceledException) { }
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
