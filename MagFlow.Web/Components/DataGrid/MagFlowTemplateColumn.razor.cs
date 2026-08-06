using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Linq.Expressions;

namespace MagFlow.Web.Components.DataGrid
{
    public class MagFlowTemplateColumn<T, TFilter> : TemplateColumn<T>
    {
        [Parameter]
        public Expression<Func<T, string>>? PropertyExpression { get; set; }

        [Parameter]
        public bool MagFlowSortable { get; set; } = false;

        [Parameter]
        public IEnumerable<TFilter>? IncludedEnumValues { get; set; }

        [Parameter]
        public IEnumerable<TFilter>? ExcludedEnumValues { get; set; }

        protected override void OnInitialized()
        {
            Filterable = true;
            Sortable = false;
            FilterTemplate = context => builder =>
            {
                builder.OpenComponent<MagFlowColumnFilter<T, string, TFilter>>(0);
                builder.AddAttribute(1, "Context", context);
                builder.AddAttribute(2, "IncludedEnumValues", IncludedEnumValues);
                builder.AddAttribute(3, "ExcludedEnumValues", ExcludedEnumValues);
                builder.CloseComponent();
            };

            if (string.IsNullOrEmpty(HeaderClass))
                HeaderClass = "text-nowrap";

            if (string.IsNullOrEmpty(CellClass))
                CellClass = "text-nowrap";

            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (PropertyExpression != null)
            {
                SortBy = PropertyExpression.Compile();
            }

            HeaderTemplate = MagFlowColumnHeader.GetHeaderTemplate(this, PropertyExpression, MagFlowSortable);
        }
    }
}
