using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Domain
{
    public class Configs : Dictionary<Type, CodeConfig>
    {
        public static CodeConfig GetConfig(ICodeEntity baseEntity)
        {
            var entityType = baseEntity.GetType();

            if (entityType == typeof(Item))
                return GetConfig(baseEntity, 0);

            return Domain.Configs.Instance.TryGetValue(entityType, out var config)
                ? config
                : new CodeConfig { Prefix = "MFO", IncludeYear = false, MinDigits = 4 };
        }

        public static CodeConfig GetConfig(ICodeEntity baseEntity, int companyNumber)
        {
            var entityType = baseEntity.GetType();

            if (entityType == typeof(Item))
            {
                var base35 = Shared.Generators.Base35Generator.ConvertObfuscated(companyNumber);
                return new CodeConfig { Prefix = $"MFW-{base35}", IncludeYear = false, MinDigits = 6 };
            }

            return Domain.Configs.Instance.TryGetValue(entityType, out var config)
                ? config
                : new CodeConfig { Prefix = "MFO", IncludeYear = false, MinDigits = 4 };
        }

        public static readonly Configs Instance = new()
        {
            { typeof(Item), Code("MFW", minDigits: 6) },
            { typeof(Product), Code("PRD") },
            { typeof(Contractor), Code("CTR") },

            { typeof(Machine), Code("MCH") },
            { typeof(MachineModel), Code("MMD") },
            { typeof(MachineFunction), Code("MCF") },
            { typeof(MachineParameter), Code("MCP") },
            { typeof(FunctionParameter), Code("FPR") },
            { typeof(CustomParameter), Code("CPR") },

            { typeof(Process), Code("PRC", includeYear: true) },
            { typeof(Document), Code("DOC", includeYear: true) },
            { typeof(Order), Code("ORD", includeYear: true) },

            { typeof(DocumentType), Code("DT") },
            { typeof(OrderType), Code("OT") },
            { typeof(ProductType), Code("PT") },
            { typeof(ProductCategory), Code("PC") },
            { typeof(Unit), Code("PU") },

            { typeof(Warehouse), Code("WH", minDigits: 2) },
            { typeof(WarehouseSector), Code("WSC", minDigits: 2) },
            { typeof(WarehouseSectorRow), Code("WSR", minDigits: 2) },
            { typeof(WarehouseSectorRowSlot), Code("WSL", minDigits: 3) }
        };

        private static CodeConfig Code(string prefix, bool includeYear = false, int minDigits = 4)
            => new() { Prefix = prefix, IncludeYear = includeYear, MinDigits = minDigits };
    }
}
