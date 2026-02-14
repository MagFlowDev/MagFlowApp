using MagFlow.Shared.DTOs.Core;
using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.BLL.Services.Interfaces
{
    public interface ILocalCacheService
    {
        Task<Enums.Result> SetSessionOrder(Guid sessionId, List<Guid> orderedIds);
        Task<List<Guid>?> GetSessionOrder(Guid sessionId);


        Task<Enums.Result> SetCurrentUser(UserDTO userDTO);
        Task<UserDTO?> GetCurrentUser();
    }
}
