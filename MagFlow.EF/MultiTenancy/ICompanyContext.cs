using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.EF.MultiTenancy
{
    public interface ICompanyContext
    {
        string? ConnectionString { get; }

        Task SetCompanyContext(string userEmail);
    }
}
