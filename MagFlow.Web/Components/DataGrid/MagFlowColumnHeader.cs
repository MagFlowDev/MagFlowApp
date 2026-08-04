using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using System.Linq.Expressions;

namespace MagFlow.Web.Components.DataGrid
{
    public static class MagFlowColumnHeader
    {
        public static RenderFragment<HeaderContext<T>>? GetHeaderTemplate<T, TProperty>(this Column<T> column, Expression<Func<T, TProperty>>? property)
        {
            return headerContext => builder =>
            {
                int cSeq = 1;
                builder.OpenElement(cSeq++, "div");
                builder.AddAttribute(cSeq++, "class", "mud-datagrid-column-header");
                builder.AddAttribute(cSeq++, "style", "display: flex; align-items: center; width: 100%; justify-content: space-between; gap: 8px;");

                builder.OpenElement(cSeq++, "div");
                if (property != null && column.Sortable == true)
                {
                    builder.AddAttribute(cSeq++, "onclick", EventCallback.Factory.Create<MouseEventArgs>(column, _ => ToggleColumnSortAsync(column, property)));
                    builder.AddAttribute(cSeq++, "style", "cursor: pointer; flex-grow: 1; display: flex; align-items: center; gap: 4px; padding: 16px 0;");
                }
                else
                {
                    builder.AddAttribute(cSeq++, "style", "flex-grow: 1; display: flex; align-items: center; gap: 4px; padding: 16px 0;");
                }

                builder.OpenElement(cSeq++, "span");
                builder.AddContent(cSeq++, column.Title ?? (property != null ? property.Type.Name : ""));
                builder.CloseElement();

                string sortKey = !string.IsNullOrEmpty(column.Title) ? column.Title : typeof(T).Name;

                if (property != null && column.Sortable == true)
                {
                    var currentSortIndex = column.DataGrid?.SortDefinitions.Keys.ToList().IndexOf(sortKey) ?? -1;
                    if (currentSortIndex >= 0)
                    {
                        var isDescending = column.DataGrid!.SortDefinitions[sortKey].Descending;

                        builder.OpenComponent<MudIcon>(cSeq++);
                        builder.AddAttribute(cSeq++, "Icon", isDescending ? Icons.Material.Filled.ArrowDownward : Icons.Material.Filled.ArrowUpward);
                        builder.AddAttribute(cSeq++, "Size", Size.Small);
                        builder.CloseComponent();
                    }
                }
                builder.CloseElement();

                if (column.Filterable == true)
                {
                    builder.OpenComponent<MudIconButton>(cSeq++);
                    builder.AddAttribute(cSeq++, "Icon", Icons.Material.Filled.FilterAlt);
                    builder.AddAttribute(cSeq++, "Size", Size.Small);
                    builder.AddAttribute(cSeq++, "OnClick", EventCallback.Factory.Create<MouseEventArgs>(column, _ => ToggleGridFilters(column)));
                    builder.CloseComponent();
                }

                builder.CloseElement();
            };
        }

        private static async Task ToggleColumnSortAsync<T, TProperty>(Column<T> column, Expression<Func<T, TProperty>> property)
        {
            if (column.DataGrid == null)
                return;

            string sortKey = !string.IsNullOrEmpty(column.PropertyName) ? column.PropertyName : (!string.IsNullOrEmpty(column.Title) ? column.Title : typeof(T).Name);

            var parameter = Expression.Parameter(typeof(T), "x");
            var propertyAccess = Expression.Invoke(property, parameter);
            var castToObject = Expression.Convert(propertyAccess, typeof(object));
            var sortByExpression = Expression.Lambda<Func<T, object>>(castToObject, parameter);

            Func<T, object?> sortByFunc = sortByExpression.Compile();

            if (column.DataGrid.SortDefinitions.TryGetValue(sortKey, out var currentSort))
            {
                if (!currentSort.Descending)
                {
                    await column.DataGrid.ExtendSortAsync(sortKey, SortDirection.Descending, sortByFunc, null);
                }
                else
                {
                    await column.DataGrid.RemoveSortAsync(sortKey);
                }
            }
            else
            {
                await column.DataGrid.ExtendSortAsync(sortKey, SortDirection.Ascending, sortByFunc, null);
            }
        }


        private static void ToggleGridFilters<T>(Column<T> column)
        {
            column.DataGrid?.ToggleFiltersMenu();
        }
    }
}
