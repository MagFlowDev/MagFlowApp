using MagFlow.Web.Components.Buttons;
using MagFlow.Web.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using MudBlazor;
using MudBlazor.Extensions;

namespace MagFlow.Web.Components.Stepper
{
    public class MagFlowStepper : MudStepper
    {
        [Parameter] public EventCallback OnCancel { get; set; }
        [Parameter] public EventCallback OnReset { get; set; }
        [Parameter] public EventCallback OnSave { get; set; }
        [Parameter] public bool IsLoading { get; set; }

        [Inject] protected IJSRuntime JS { get; set; } = null!;
        [Inject] protected IStringLocalizer<Langs> Localizer { get; set; } = null!;

        protected override void OnInitialized()
        {
            NonLinear = true;
            Class = "flex-grow-1 d-flex flex-column h-100";
            Style = "min-height: 0";
            StepClass = "d-flex flex-column flex-grow-1";
            NavClass = "minh-72px";

            ActionContent = (stepper) => (builder) =>
            {
                int seq = 0;
                var currentIndex = this.GetState(x => x.ActiveIndex);
                if(currentIndex == 0)
                {
                    builder.OpenComponent<MudButton>(seq++);
                    builder.AddAttribute(seq++, "Variant", Variant.Text);
                    builder.AddAttribute(seq++, "Color", Color.Default);
                    builder.AddAttribute(seq++, "OnClick", EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, () => OnCancel.InvokeAsync()));
                    builder.AddAttribute(seq++, "ChildContent", (RenderFragment)((b) => b.AddContent(seq++, Localizer != null ? Localizer[Langs.Cancel] : string.Empty)));
                    builder.CloseComponent();
                }
                else
                {
                    builder.OpenComponent<MudButton>(seq++);
                    builder.AddAttribute(seq++, "StartIcon", Icons.Material.Outlined.FirstPage);
                    builder.AddAttribute(seq++, "Color", Color.Default);
                    builder.AddAttribute(seq++, "OnClick", EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, () => OnReset.InvokeAsync()));
                    builder.AddAttribute(seq++, "ChildContent", (RenderFragment)((b) => b.AddContent(seq++, Localizer != null ? Localizer[Langs.Reset] : string.Empty)));
                    builder.CloseComponent();

                    builder.OpenComponent<MudButton>(seq++);
                    builder.AddAttribute(seq++, "StartIcon", Icons.Material.Outlined.NavigateBefore);
                    builder.AddAttribute(seq++, "Color", Color.Default);
                    builder.AddAttribute(seq++, "OnClick", EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, async () => await stepper.PreviousStepAsync()));
                    builder.AddAttribute(seq++, "ChildContent", (RenderFragment)((b) => b.AddContent(seq++, Localizer != null ? Localizer[Langs.LastStep] : string.Empty)));
                    builder.CloseComponent();
                }
                builder.OpenComponent<MudSpacer>(seq++);
                builder.CloseComponent();
                if(currentIndex == (stepper.Steps.Count - 1))
                {
                    builder.OpenComponent<LoadingButton>(seq++);
                    builder.AddAttribute(seq++, "Variant", Variant.Filled);
                    builder.AddAttribute(seq++, "Color", Color.Primary);
                    builder.AddAttribute(seq++, "Loading", IsLoading);
                    builder.AddAttribute(seq++, "OnClick", EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, () => OnSave.InvokeAsync()));
                    builder.AddAttribute(seq++, "ChildContent", (RenderFragment)((b) => b.AddContent(seq++, Localizer != null ? Localizer[Langs.Save] : string.Empty)));
                    builder.CloseComponent();
                }
                else
                {
                    builder.OpenComponent<MudButton>(seq++);
                    builder.AddAttribute(seq++, "Variant", Variant.Text);
                    builder.AddAttribute(seq++, "Color", Color.Primary);
                    builder.AddAttribute(seq++, "StartIcon", Icons.Material.Outlined.NavigateNext);
                    builder.AddAttribute(seq++, "OnClick", EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, async () => await stepper.NextStepAsync()));
                    builder.AddAttribute(seq++, "ChildContent", (RenderFragment)((b) => b.AddContent(seq++, Localizer != null ? Localizer[Langs.NextStep] : string.Empty)));
                    builder.CloseComponent();
                }
            };

            base.OnInitialized();
        }
    }
}
