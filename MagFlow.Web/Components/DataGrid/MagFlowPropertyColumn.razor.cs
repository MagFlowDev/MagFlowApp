using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using System.Linq.Expressions;

namespace MagFlow.Web.Components.DataGrid
{
    public class MagFlowPropertyColumn<T, TProperty, TFilter> : PropertyColumn<T, TProperty>
    {
        [Parameter]
        public bool MagFlowSortable { get; set; } = false;

        protected override void OnInitialized()
        {
            Filterable = true;
            Sortable = false;
            FilterTemplate = context => builder =>
            {
                builder.OpenComponent<MagFlowColumnFilter<T, TProperty, TFilter>>(0);
                builder.AddAttribute(1, "Context", context);
                builder.CloseComponent();
            };

            if(string.IsNullOrEmpty(HeaderClass))
                HeaderClass = "text-nowrap";

            if (string.IsNullOrEmpty(CellClass))
                CellClass = "text-nowrap";

            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            HeaderTemplate = MagFlowColumnHeader.GetHeaderTemplate(this, Property, MagFlowSortable);
        }
    }
}
