using System;
using System.Collections.Generic;
using System.Text;
using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.DTOs.CompanyScope;

namespace MagFlow.BLL.Mappers.Domain.CompanyScope
{
    public static class ItemMapper
    {
        public static ItemDTO ToDTO(this Item item)
        {
            return new ItemDTO()
            {
                Id = item.Id,
                ExternalId = item.ExternalId,
                CreatedAt = item.CreatedAt,
                Quantity = item.Quantity,
                Status = item.Status
            };
        }
    }
}
