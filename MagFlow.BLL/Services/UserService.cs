using MagFlow.BLL.Mappers.Domain;
using MagFlow.BLL.Mappers.Domain.Core;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.DAL.Repositories.Core.Interfaces;
using MagFlow.Shared.DTOs.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository,
            ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<UserDTO?> GetUser(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user?.ToDTO();
        }
    }
}
