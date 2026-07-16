using MagFlow.Shared.DTOs.CompanyScope;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Helpers
{
    public static class UnitHelper
    {
        public static decimal Convert(this ItemDTO item, UnitDTO targetUnit)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (targetUnit == null) throw new ArgumentNullException(nameof(targetUnit));
            if (item.Unit == null) throw new InvalidOperationException("Item does not have a source unit.");

            var rate = FindConversionRate(item.Unit, targetUnit);
            return item.Quantity * rate;
        }

        public static decimal ConvertValue(decimal quantity, UnitDTO sourceUnit, UnitDTO targetUnit)
        {
            if (sourceUnit == null || targetUnit == null)
                throw new ArgumentNullException("Unit cannot be null.");

            decimal rate = FindConversionRate(sourceUnit, targetUnit);
            return quantity * rate;
        }

        public static decimal FindConversionRate(UnitDTO source, UnitDTO target)
        {
            if (source.Id == target.Id) return 1m;

            var queue = new Queue<(UnitDTO Unit, decimal CurrentRate)>();
            var visited = new HashSet<int>();

            queue.Enqueue((source, 1m));
            visited.Add(source.Id);

            while (queue.Count > 0)
            {
                var (current, currentRate) = queue.Dequeue();
                if (current.ParentUnit != null && !visited.Contains(current.ParentUnit.Id))
                {
                    if (current.ParentUnitConversionRate is null or 0)
                        throw new InvalidOperationException($"Invalid conversion rate for unit {current.Name}.");


                    decimal nextRate = currentRate * (1m / current.ParentUnitConversionRate.Value);

                    if (current.ParentUnit.Id == target.Id) return nextRate;

                    queue.Enqueue((current.ParentUnit, nextRate));
                    visited.Add(current.ParentUnit.Id);
                }

                if (current.RelatedUnits != null)
                {
                    foreach (var child in current.RelatedUnits)
                    {
                        if (!visited.Contains(child.Id))
                        {
                            if (child.ParentUnitConversionRate is null)
                                throw new InvalidOperationException($"Cannot find conversion rate for unit {child.Name}.");

                            decimal nextRate = currentRate * child.ParentUnitConversionRate.Value;

                            if (child.Id == target.Id) return nextRate;

                            queue.Enqueue((child, nextRate));
                            visited.Add(child.Id);
                        }
                    }
                }
            }

            throw new InvalidOperationException($"Cannot find conversion path from {source.Name} to {target.Name}.");
        }
    }
}
