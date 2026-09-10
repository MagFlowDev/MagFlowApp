using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.DTOs.CompanyScope;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Services.Interfaces
{
    public interface IStockMovementService : IBaseCompanyService<StockMovement, StockMovementDTO>
    {
    }
}
