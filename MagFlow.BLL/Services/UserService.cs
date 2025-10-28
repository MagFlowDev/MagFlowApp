using MagFlow.BLL.Services.Interfaces;
using MagFlow.DAL.Repositories.Interfaces;
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
    }
}
