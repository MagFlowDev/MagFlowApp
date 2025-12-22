using MagFlow.DAL.Repositories.Company.Interfaces;
using MagFlow.Domain.Company;
using MagFlow.EF;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.DAL.Repositories.Company
{
    public class DocumentRepository : BaseCompanyRepository<Document, DocumentRepository>, IDocumentRepository
    {
        public DocumentRepository(ICoreDbContextFactory coreContextFactory, 
            ICompanyDbContextFactory companyContextFactory, 
            ILogger<DocumentRepository> logger) : base(coreContextFactory, companyContextFactory, logger)
        {
        }
    }
}
