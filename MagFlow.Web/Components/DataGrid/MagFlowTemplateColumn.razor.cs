using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Linq.Expressions;

namespace MagFlow.Web.Components.DataGrid
{
    public class MagFlowTemplateColumn<T, TFilter> : TemplateColumn<T>
    {
        [Parameter]
        public Expression<Func<T, string>>? PropertyExpression { get; set; }

        protected override void OnInitialized()
        {
            Filterable = true;
            FilterTemplate = context => builder =>
            {
                builder.OpenComponent<MagFlowColumnFilter<T, string, TFilter>>(0);
                builder.AddAttribute(1, "Context", context);
                builder.CloseComponent();
            };

            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (PropertyExpression != null)
            {
                SortBy = PropertyExpression.Compile();
            }

            HeaderTemplate = MagFlowColumnHeader.GetHeaderTemplate(this, PropertyExpression);
        }
    }
}
