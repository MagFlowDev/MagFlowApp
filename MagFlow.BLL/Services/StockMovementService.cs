using MagFlow.BLL.Services.Interfaces;
using MagFlow.DAL.Repositories.CompanyScope.Interfaces;
using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.DTOs.CompanyScope;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Services
{
    public class StockMovementService : BaseCompanyService<StockMovement, StockMovementDTO>, IStockMovementService
    {
        private readonly IStockMovementRepository _stockMovementRepository;

        private readonly INetworkService _networkService;

        public StockMovementService(IStockMovementRepository stockMovementRepository,
            INetworkService networkService) : base(stockMovementRepository, networkService)
        {
            _stockMovementRepository = stockMovementRepository;
            _networkService = networkService;
        }
    }
}
