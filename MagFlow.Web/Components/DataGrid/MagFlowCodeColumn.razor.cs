using MagFlow.Shared.Models.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace MagFlow.Web.Components.DataGrid
{
    public class MagFlowCodeColumn<T> : MagFlowTemplateColumn<T> where T : ICodeDTO
    {
        protected override void OnInitialized()
        {
            if (string.IsNullOrEmpty(CellClass)) CellClass = "text-nowrap";
            Sortable = true;
            SortBy = item => item.Code;

            CellTemplate = context => (builder) =>
            {
                var code = context.Item?.Code;
                var lastDashIndex = code?.LastIndexOf('-') ?? -1;
                int seq = 0;

                builder.OpenElement(seq++, "div");
                builder.AddAttribute(seq++, "style", "width: 100%; height: 100%; display: inline-block;");

                builder.AddEventStopPropagationAttribute(seq++, "onclick", true);
                builder.AddEventStopPropagationAttribute(seq++, "onmousedown", true);

                if (lastDashIndex != -1)
                {

                    builder.OpenElement(seq++, "span");
                    builder.AddAttribute(seq++, "style", "color: #aaaaaa;");
                    builder.AddContent(seq++, code!.Substring(0, lastDashIndex + 1));
                    builder.CloseElement();

                    builder.OpenElement(seq++, "span");
                    builder.AddContent(seq++, code.Substring(lastDashIndex + 1));
                    builder.CloseElement();
                }
                else
                {
                    builder.AddContent(0, code);
                }

                builder.CloseElement();
            };

            base.OnInitialized();
        }
    }
}
