using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using MudBlazor;

namespace MagFlow.Web.Components.Stepper
{
    public class MagFlowStep : MudStep
    {
        protected override void OnInitialized()
        {
            Style = "min-height: 0; height: 100%;";

            var originalChildContent = ChildContent;

            ChildContent = (builder) =>
            {
                int seq = 0;

                builder.OpenElement(seq++, "div");
                builder.AddAttribute(seq++, "style", "overflow-y: auto; flex-grow: 1; min-height: 0;");
                builder.AddAttribute(seq++, "class", "d-flex flex-column align-items-center pt-3");

                builder.OpenElement(seq++, "div");
                builder.AddAttribute(seq++, "class", "d-flex flex-column align-items-start w-100");
                builder.AddAttribute(seq++, "style", "gap: 24px; max-width: 550px;");

                builder.AddContent(seq++, originalChildContent);

                builder.CloseElement();
                builder.CloseElement();
            };

            base.OnInitialized();
        }
    }
}
