using MagFlow.BLL.Services.Interfaces;
using MagFlow.DAL.Repositories.CompanyScope.Interfaces;
using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.DTOs.CompanyScope;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Services
{
    public class ContractorService : BaseCompanyService<Contractor, ContractorDTO>, IContractorService
    {
        private readonly IContractorRepository _contractorRepository;

        private readonly INetworkService _networkService;

        public ContractorService(IContractorRepository contractorRepository,
            INetworkService networkService,
            ILogger<ContractorService> logger) : base(contractorRepository, networkService, logger)
        {
            _contractorRepository = contractorRepository;
            _networkService = networkService;
        }
    }
}
