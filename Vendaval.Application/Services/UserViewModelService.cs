using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Infrastructure.Data.Repositories.EFRepositories.Interfaces;
using Vendaval.Infrastructure.Data.Repositories.RedisRepositories.Interfaces;

namespace Vendaval.Application.Services
{
    public class UserViewModelService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRedisRepository _redisRepository;
        public UserViewModelService(IUserRepository userRepository, IRedisRepository redisRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _redisRepository = redisRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }


        


    }
}
