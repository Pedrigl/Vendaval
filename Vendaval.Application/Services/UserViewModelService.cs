using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Infrastructure.Data.Repositories.EFRepositories.Interfaces;

namespace Vendaval.Application.Services
{
    public class UserViewModelService
    {
        private readonly IUserRepository _userRepository;
        private readonly IDatabase _redisDatabase;
        public UserViewModelService(IUserRepository userRepository, IDatabase)
        {
            _userRepository = userRepository;
        }
        


    }
}
