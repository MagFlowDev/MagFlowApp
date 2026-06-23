using MagFlow.BLL.Helpers;
using MagFlow.BLL.Services;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.Shared.DTOs.CoreScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.FormModels;
using MagFlow.Web.Pages.Modules;
using MagFlow.Web.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;

namespace MagFlow.Web.Components.Wizards
{
    public abstract class StepperWizardBase<T> : AuthComponentBase
    {
        protected ILocalCacheService LocalCacheService { get; private set; } = default!;
        protected IServiceProvider Services { get; private set; } = default!;
        protected IJSRuntime JS { get; private set; } = default!;
        protected ISnackbar Snackbar { get; private set; } = default!;
        protected NavigationManager NavigationManager { get; private set; } = default!;

        protected void SetServices(ILocalCacheService localCacheService,
            IServiceProvider services,
            IJSRuntime js,
            ISnackbar snackbar,
            NavigationManager navigationManager)
        {
            LocalCacheService = localCacheService;
            Services = services;
            JS = js;
            Snackbar = snackbar;
            NavigationManager = navigationManager;
        }

        protected virtual T _model { get; set; } = default!;
        
        protected MudStepper _stepper;
        protected EditContext _context;
        protected ValidationMessageStore _messageStore;

        protected Dictionary<int, Func<object>> _stepSections;
        protected int _step;

        protected UserDTO? _user = null;

        protected bool _isBusy;
        protected bool _loading;


        protected override async Task OnInitializedAsync()
        {
            base.OnInitialized();

            _model = CreateModel();
            _context = new EditContext(_model);
            _messageStore = new ValidationMessageStore(_context);
            _stepSections = new()
            {
            };

            _user = await LocalCacheService.GetCurrentUser();
        }

        protected virtual async Task ResetForm(MudStepper stepper)
        {
            await stepper.ResetAsync();
            _model = CreateModel();
            _context = new EditContext(_model);
            _messageStore = new ValidationMessageStore(_context);
            await InvokeAsync(StateHasChanged);
        }

        protected virtual void StepChanged(int value)
        {
            _step = value;
        }

        protected virtual async Task Save()
        {
            if (_isBusy)
                return;

            var step = _stepper.Steps[_step];
            if (step == null || !await ValidateStep(_step))
                return;
            await step.SetCompletedAsync(true);
        }

        protected virtual async Task Cancel(MudStepper stepper)
        {
            if (_isBusy)
                return;

            await stepper.ResetAsync();
            await JS.InvokeVoidAsync("history.back");
        }

        protected virtual async Task ControlStepCompletion(StepperInteractionEventArgs arg)
        {
            var step = _stepper.Steps[arg.StepIndex];
            if (!await ValidateStep(arg.StepIndex))
                arg.Cancel = true;
        }

        protected virtual async Task ControlStepNavigation(StepperInteractionEventArgs arg)
        {
            if (arg.StepIndex == 0)
                return;
            for (int i = 0; i <= arg.StepIndex - 1; i++)
            {
                var step = _stepper.Steps[i];
                if (!step.Completed)
                    arg.Cancel = true;
            }
        }

        protected virtual async Task OnPreviewInteraction(StepperInteractionEventArgs arg)
        {
            var currentStep = _stepper.Steps[_step];
            if (currentStep.Completed && arg.StepIndex != _step)
            {
                if (!await ValidateStep(_step))
                {
                    await currentStep.SetCompletedAsync(false);
                    arg.Cancel = true;
                    return;
                }
            }

            if (arg.Action == StepAction.Complete)
                await ControlStepCompletion(arg);
            else if (arg.Action == StepAction.Activate)
                await ControlStepNavigation(arg);
        }

        protected virtual async Task<bool> ValidateStep(int step)
        {
            if (!_stepSections.TryGetValue(step, out var section))
                return false;
            var isValid = await _messageStore.IsSectionValid(Services, section());
            _context.NotifyValidationStateChanged();
            return isValid;
        }

        protected virtual T CreateModel()
        {
            return Activator.CreateInstance<T>();
        }
    }
}
