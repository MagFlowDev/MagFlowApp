using MagFlow.DAL.Repositories.Interfaces;
using MagFlow.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ICoreDbContextFactory _coreContextFactory;
        private readonly ICompanyDbContextFactory _companyContextFactory;

        public UserRepository(ICoreDbContextFactory coreContextFactory,
            ICompanyDbContextFactory companyContextFactory)
        {
            _coreContextFactory = coreContextFactory;
            _companyContextFactory = companyContextFactory;
        }

        void Test()
        {
            using(var context = _coreContextFactory.CreateDbContext())
            {

            }
            using(var context = _companyContextFactory.CreateDbContext(""))
            {

            }
        }
    }
}
