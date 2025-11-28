using MagFlow.Shared.DTOs.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> GetUser(Guid id);
    }
}
