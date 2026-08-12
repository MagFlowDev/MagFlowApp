using MagFlow.Shared.DTOs;
using MudBlazor;
using Org.BouncyCastle.Asn1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.Shared.Models
{
    public class SessionCache
    {
        public Guid SessionId { get; set; }
        public List<Guid> SessionOrder { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }

    public class SessionCurrentModule
    {
        public Guid SessionId { get; set; }
        public Guid ModuleId { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }

    public class SessionModuleSection
    {
        public Guid SessionId { get; set; }
        public Guid ModuleId { get; set; }
        public string Section { get; set; }
        public string SectionType { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }

    public class SessionTableFilters
    {
        public Guid SessionId { get; set; }
        public string TableId { get; set; }
        public bool FiltersDisplayed { get; set; }
        public List<FilterDefinitionDTO> Filters { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }
}
