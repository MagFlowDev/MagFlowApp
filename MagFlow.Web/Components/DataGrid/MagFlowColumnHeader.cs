using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using System.Linq.Expressions;

namespace MagFlow.Web.Components.DataGrid
{
    public static class MagFlowColumnHeader
    {
        public static RenderFragment<HeaderContext<T>>? GetHeaderTemplate<T, TProperty>(this Column<T> column, Expression<Func<T, TProperty>>? property, bool sortable = false)
        {
            bool canSort = property != null && sortable;

            return headerContext => builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddAttribute(1, "class", "mud-datagrid-column-header");
                builder.AddAttribute(2, "style", "display: flex; align-items: center; width: 100%; justify-content: space-between; gap: 8px;");

                builder.OpenElement(3, "div");
                if (property != null && sortable)
                {
                    builder.AddAttribute(4, "onclick", EventCallback.Factory.Create<MouseEventArgs>(column, _ => ToggleColumnSortAsync(column, property)));
                    builder.AddAttribute(5, "style", "cursor: pointer; flex-grow: 1; display: flex; align-items: center; gap: 4px; padding: 16px 0;");
                }
                else
                {
                    builder.AddAttribute(6, "style", "flex-grow: 1; display: flex; align-items: center; gap: 4px; padding: 16px 0;");
                }

                builder.OpenElement(7, "span");
                builder.AddContent(8, column.Title ?? (property != null ? property.Type.Name : ""));
                builder.CloseElement();

                string sortKey = !string.IsNullOrEmpty(column.Title) ? column.Title : typeof(T).Name;

                if (canSort && column.UserAttributes.TryGetValue("MagFlowSortDirection", out var directionObj) && directionObj is SortDirection currentDir && currentDir != SortDirection.None)
                {
                    bool isDescending = currentDir == SortDirection.Descending;

                    builder.OpenComponent<MudIconButton>(9);
                    builder.AddAttribute(10, "Icon", isDescending ? Icons.Material.Filled.ArrowDownward : Icons.Material.Filled.ArrowUpward);
                    builder.AddAttribute(11, "Size", Size.Small);
                    builder.AddAttribute(12, "Class", "is-sorted");
                    builder.AddAttribute(13, "OnClick", EventCallback.Factory.Create<MouseEventArgs>(column, _ => ToggleColumnSortAsync(column, property, true)));
                    builder.CloseComponent();
                }
                builder.CloseElement();

                if (column.Filterable == true && (column.DataGrid == null || column.DataGrid.Filterable == true))
                {
                    if (column.FilterContext.FilterDefinition?.Value != null)
                    {
                        builder.OpenComponent<MudIconButton>(14);
                        builder.AddAttribute(15, "Icon", Icons.Material.Filled.FilterAltOff);
                        builder.AddAttribute(16, "Size", Size.Small);
                        builder.AddAttribute(17, "OnClick", EventCallback.Factory.Create<MouseEventArgs>(column, _ => ToggleGridFilters(column, false)));
                        builder.CloseComponent();
                    }
                    else
                    {
                        builder.OpenComponent<MudIconButton>(14);
                        builder.AddAttribute(15, "Icon", Icons.Material.Outlined.FilterAltOff);
                        builder.AddAttribute(16, "Size", Size.Small);
                        builder.AddAttribute(17, "OnClick", EventCallback.Factory.Create<MouseEventArgs>(column, _ => ToggleGridFilters(column, false)));
                        builder.CloseComponent();
                    }
                }
                else if(column.DataGrid?.Filterable == false)
                {
                    if(column.FilterContext.FilterDefinition?.Value != null)
                    {
                        builder.OpenComponent<MudIconButton>(14);
                        builder.AddAttribute(15, "Icon", Icons.Material.Filled.FilterAlt);
                        builder.AddAttribute(16, "Size", Size.Small);
                        builder.AddAttribute(17, "OnClick", EventCallback.Factory.Create<MouseEventArgs>(column, _ => ToggleGridFilters(column, true)));
                        builder.CloseComponent();
                    }
                    else
                    {
                        builder.OpenComponent<MudIconButton>(14);
                        builder.AddAttribute(15, "Icon", Icons.Material.Outlined.FilterAlt);
                        builder.AddAttribute(16, "Size", Size.Small);
                        builder.AddAttribute(17, "OnClick", EventCallback.Factory.Create<MouseEventArgs>(column, _ => ToggleGridFilters(column, true)));
                        builder.CloseComponent();
                    }
                }

                builder.CloseElement();
            };
        }

        private static async Task ToggleColumnSortAsync<T, TProperty>(Column<T> column, Expression<Func<T, TProperty>> property, bool iconClicked = false)
        {
            if (column.DataGrid == null)
                return;

            string sortKey = !string.IsNullOrEmpty(column.PropertyName) ? column.PropertyName : (!string.IsNullOrEmpty(column.Title) ? column.Title : typeof(T).Name);

            var parameter = Expression.Parameter(typeof(T), "x");
            var propertyAccess = Expression.Invoke(property, parameter);
            var castToObject = Expression.Convert(propertyAccess, typeof(object));
            var sortByExpression = Expression.Lambda<Func<T, object>>(castToObject, parameter);

            Func<T, object?> sortByFunc = sortByExpression.Compile();

            SortDirection currentDir = SortDirection.None;
            if (column.UserAttributes.TryGetValue("MagFlowSortDirection", out var dirObj) && dirObj is SortDirection existingDir)
            {
                currentDir = existingDir;
            }

            if (currentDir == SortDirection.None)
            {
                foreach (var col in column.DataGrid.RenderedColumns)
                {
                    if (col != column)
                    {
                        if (col.UserAttributes.ContainsKey("MagFlowSortDirection"))
                        {
                            col.UserAttributes["MagFlowSortDirection"] = SortDirection.None;
                        }
                    }
                }

                column.DataGrid.SortDefinitions.Clear();

                column.UserAttributes["MagFlowSortDirection"] = SortDirection.Ascending;
                await column.DataGrid.ExtendSortAsync(sortKey, SortDirection.Ascending, sortByFunc, null);
            }
            else if (currentDir == SortDirection.Ascending)
            {
                column.UserAttributes["MagFlowSortDirection"] = SortDirection.Descending;
                await column.DataGrid.ExtendSortAsync(sortKey, SortDirection.Descending, sortByFunc, null);
            }
            else if (currentDir == SortDirection.Descending && iconClicked)
            {
                column.UserAttributes["MagFlowSortDirection"] = SortDirection.Ascending;
                await column.DataGrid.ExtendSortAsync(sortKey, SortDirection.Ascending, sortByFunc, null);
            }
            else
            {
                column.UserAttributes["MagFlowSortDirection"] = SortDirection.None;
                await column.DataGrid.RemoveSortAsync(sortKey);
            }

            ForceTableStateUpdate(column);
        }


        private static void ToggleGridFilters<T>(Column<T> column, bool enable)
        {
            if (column.DataGrid == null)
                return;

            if (column.DataGrid is MagFlowDataGrid<T> magFlowDataGrid)
            {
                magFlowDataGrid.SetFilterableImperatively(enable);
            }
            else
            {
                column.DataGrid.Filterable = enable;
            }
            ForceTableStateUpdate(column);
        }

        private static void ForceTableStateUpdate<T>(Column<T> column)
        {
            if (column.DataGrid == null)
                return;

            var stateHasChangedMethod = typeof(MudDataGrid<T>)
                .GetMethod("StateHasChanged", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            stateHasChangedMethod?.Invoke(column.DataGrid, null);
        }
    }
}
