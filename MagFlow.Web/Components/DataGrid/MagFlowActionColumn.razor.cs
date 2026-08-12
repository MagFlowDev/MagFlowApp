using MudBlazor;
using MudBlazor.Charts;

namespace MagFlow.Web.Components.DataGrid
{
    public class MagFlowActionColumn<T> : TemplateColumn<T>
    {
        protected override void OnInitialized()
        {
            Sortable = false;
            Filterable = true;
            FilterTemplate = context => builder =>
            {
                builder.OpenComponent<MagFlowActionColumnFilter<T>>(0);
                builder.AddAttribute(1, "Context", context);
                builder.CloseComponent();
            };

            if (string.IsNullOrEmpty(HeaderClass))
                HeaderClass = "text-nowrap";

            if (string.IsNullOrEmpty(CellClass))
                CellClass = "text-nowrap";

            base.OnInitialized();
        }
    }
}
