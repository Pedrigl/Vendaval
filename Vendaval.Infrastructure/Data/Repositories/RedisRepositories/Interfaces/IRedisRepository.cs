using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendaval.Infrastructure.Data.Repositories.RedisRepositories.Interfaces
{
    public interface IRedisRepository
    {
        Task<RedisValue> GetValueAsync(string key);
        Task<RedisValue> SetValueAsync(string key, string value, TimeSpan? expiration = null);
        Task<bool> RemoveValueAsync(string key);
    }
}
