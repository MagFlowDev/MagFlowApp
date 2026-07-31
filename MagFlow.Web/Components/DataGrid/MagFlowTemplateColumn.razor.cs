using MudBlazor;

namespace MagFlow.Web.Components.DataGrid
{
    public class MagFlowTemplateColumn<T> : TemplateColumn<T>
    {
        protected override void OnInitialized()
        {
            Filterable = true;
            FilterTemplate = context => builder =>
            {
                builder.OpenComponent<MagFlowColumnFilter<T, string>>(0);
                builder.AddAttribute(1, "Context", context);
                builder.CloseComponent();
            };

            base.OnInitialized();
        }
    }
}
