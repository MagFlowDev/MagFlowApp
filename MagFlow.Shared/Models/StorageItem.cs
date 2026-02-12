using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.Shared.Models
{
    public class StorageItem<T>
    {
        public required string Key { get; set; }
        public required T Data { get; set; }
    }
}
