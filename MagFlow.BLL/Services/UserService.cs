using MagFlow.BLL.Mappers.Domain;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.DAL.Repositories.Core.Interfaces;
using MagFlow.Shared.DTOs.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDTO?> GetUser(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user?.ToDTO();
        }
    }
}
