using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using System.Linq.Expressions;

namespace MagFlow.Web.Components.DataGrid
{
    public class MagFlowPropertyColumn<T, TProperty, TFilter> : PropertyColumn<T, TProperty>
    {
        protected override void OnInitialized()
        {
            Filterable = true;
            FilterTemplate = context => builder =>
            {
                builder.OpenComponent<MagFlowColumnFilter<T, TProperty, TFilter>>(0);
                builder.AddAttribute(1, "Context", context);
                builder.CloseComponent();
            };

            HeaderTemplate = MagFlowColumnHeader.GetHeaderTemplate(this, Property);

            base.OnInitialized();
        }

        
    }
}
