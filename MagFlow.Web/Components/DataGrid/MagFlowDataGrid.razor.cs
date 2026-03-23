using MagFlow.Shared.DTOs.CoreScope;
using MagFlow.Web.Components.Pagination;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using MudBlazor;

namespace MagFlow.Web.Components.DataGrid
{
    public class MagFlowDataGrid<T> : MudDataGrid<T>
    {
        private T? _selectedItem;
        public T? SelectedItem
        {
            get => _selectedItem;
            private set
            {
                if (!EqualityComparer<T>.Default.Equals(_selectedItem, value))
                {
                    _selectedItem = value;
                    StateHasChanged();
                }
            }
        }

        [Parameter] public EventCallback<T> OnRowClicked { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            FixedHeader = true;
            Virtualize = true;
            Dense = true;
            RowsPerPage = 25;
            Class = "align-self-stretch mud-table-overflow-hidden flex-grow-1 dg-fixed-pager striped-grid";

            RowClick = EventCallback.Factory.Create<DataGridRowClickEventArgs<T>>(this, OnRowClick);
            RowClassFunc = SelectedRowClassFunc;

            if (PagerContent == null)
            {
                PagerContent = builder =>
                {
                    builder.OpenComponent<DataGridPager<T>>(100);
                    builder.AddAttribute(101, "DataGrid", this);
                    builder.CloseComponent();
                };
            }
        }

        private async Task OnRowClick(DataGridRowClickEventArgs<T> clickEventArgs)
        {
            SelectedItem = clickEventArgs.Item;

            var idProp = clickEventArgs.Item?.GetType().GetProperty("Id");
            var idValue = idProp?.GetValue(clickEventArgs.Item);
            if (idValue == null || (idValue is Guid g && g == Guid.Empty))
                return;

            if (clickEventArgs.MouseEventArgs.Detail == 2 && OnRowClicked.HasDelegate)
            {
                await OnRowClicked.InvokeAsync(clickEventArgs.Item);
            }
        }

        private string SelectedRowClassFunc(T item, int rowNumber)
        {
            if(SelectedItem == null || item == null)
                return "";

            return EqualityComparer<T>.Default.Equals(item, SelectedItem)
                ? "mud-table-row-selected"
                :  "";
        }
    }
}
