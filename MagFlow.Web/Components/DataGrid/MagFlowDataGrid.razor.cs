using MagFlow.BLL.Mappers;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.Shared.DTOs.CoreScope;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using System.Text.Json;

namespace MagFlow.Web.Components.DataGrid
{
    public class MagFlowDataGrid<T> : MudDataGrid<T>
    {
        [Inject] public ILocalCacheService LocalCacheService { get; set; } = default!;

        [CascadingParameter(Name = "SessionId")]
        protected Guid SessionId { get; set; }

        [Parameter, EditorRequired]
        public string DataGridId { get; set; }

        [Parameter]
        public bool DisableFiltering { get; set; } = false;

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

        private EventCallback<IReadOnlyCollection<IFilterDefinition<T>>> _originalFilterChanged;
        private bool _filterable = false;
        private string _lastSavedFiltersJson = string.Empty;
        private bool _initialized = false;

        [Parameter] public EventCallback<T> OnRowClicked { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (DisableFiltering)
                Filterable = false;
            _filterable = Filterable;
            _originalFilterChanged = base.FilterChanged;
            base.FilterChanged = EventCallback.Factory.Create<IReadOnlyCollection<IFilterDefinition<T>>>(this, HandleFiltersChanged);

            FixedHeader = true;
            Virtualize = true;
            Dense = true;
            RowsPerPage = 25;
            if (string.IsNullOrEmpty(Class))
                Class = "align-self-stretch mud-table-overflow-hidden flex-grow-1 dg-fixed-pager striped-grid overflow-x-auto";
            FilterMode = DataGridFilterMode.ColumnFilterRow;

            RowClick = EventCallback.Factory.Create<DataGridRowClickEventArgs<T>>(this, OnRowClick);
            RowClassFunc = SelectedRowClassFunc;

            if (PagerContent == null)
            {
                PagerContent = builder =>
                {
                    builder.OpenComponent<MagFlow.Web.Components.Pagination.DataGridPager<T>>(100);
                    builder.AddAttribute(101, "DataGrid", this);
                    builder.CloseComponent();
                };
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await RestoreFilters();
                _initialized = true;
            }

            await base.OnAfterRenderAsync(firstRender);
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
            if (SelectedItem == null || item == null)
                return "";

            return EqualityComparer<T>.Default.Equals(item, SelectedItem)
                ? "mud-table-row-selected"
                : "";
        }

        public void SetFilterableImperatively(bool enable)
        {
            if (DisableFiltering) return;
            if (base.Filterable == enable) return;

            base.Filterable = enable;

            if (_filterable != Filterable)
            {
                _filterable = Filterable;
                HandleFilterableChanged(Filterable);
            }

            StateHasChanged();
        }

        private async Task HandleFilterableChanged(bool filterable)
        {
            if (SessionId == null || SessionId == Guid.Empty || string.IsNullOrEmpty(DataGridId))
                return;

            await LocalCacheService.SetTableFilters(SessionId, DataGridId, filterable, this.FilterDefinitions);
        }

        private async Task HandleFiltersChanged(IReadOnlyCollection<IFilterDefinition<T>> filterDefinitions)
        {
            if (_originalFilterChanged.HasDelegate)
            {
                await _originalFilterChanged.InvokeAsync(filterDefinitions);
            }

            if (!_initialized)
                return;

            var currentFiltersDto = filterDefinitions.Select(f => new { Property = f.Column?.PropertyName, f.Operator, f.Value }).ToList();
            var currentFiltersJson = System.Text.Json.JsonSerializer.Serialize(currentFiltersDto);

            if (currentFiltersJson == _lastSavedFiltersJson)
                return;
            _lastSavedFiltersJson = currentFiltersJson;

            if (SessionId == null || SessionId == Guid.Empty || string.IsNullOrEmpty(DataGridId))
                return;

            await LocalCacheService.SetTableFilters(SessionId, DataGridId, _filterable, filterDefinitions.ToList());
        }

        private async Task RestoreFilters()
        {
            if (SessionId == null || SessionId == Guid.Empty || string.IsNullOrEmpty(DataGridId))
                return;

            var tableFilters = await LocalCacheService.GetTableFilters(SessionId, DataGridId, this);
            var filters = tableFilters.filters;
            var filterable = tableFilters.filtersDisplayed;

            if (filters == null && filterable == _filterable)
                return;

            if (filterable != base.Filterable)
            {
                if (DisableFiltering && filterable)
                    return;

                base.Filterable = filterable;
                _filterable = Filterable;
            }
            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    if (filter.Value is JsonElement jsonElement)
                    {
                        filter.Value = jsonElement.ValueKind switch
                        {
                            JsonValueKind.String => jsonElement.GetString(),
                            JsonValueKind.Number => jsonElement.TryGetInt32(out var intVal) ? intVal : jsonElement.GetDouble(),
                            JsonValueKind.True => true,
                            JsonValueKind.False => false,
                            JsonValueKind.Null => null,
                            _ => jsonElement.GetRawText()
                        };
                    }
                }
                this.FilterDefinitions = filters;
                var initialFiltersDto = this.FilterDefinitions.Select(f => new { Property = f.Column?.PropertyName, f.Operator, f.Value }).ToList();
                _lastSavedFiltersJson = System.Text.Json.JsonSerializer.Serialize(initialFiltersDto);
            }
        }

       
    }
}
