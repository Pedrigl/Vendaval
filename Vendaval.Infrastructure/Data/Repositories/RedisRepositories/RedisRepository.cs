using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Infrastructure.Data.Repositories.RedisRepositories.Interfaces;

namespace Vendaval.Infrastructure.Data.Repositories.RedisRepositories
{
    public class RedisRepository : IRedisRepository
    {
        private readonly IDatabase _redisDatabase;
        public async Task<RedisValue> GetValueAsync(string key)
        {
            return await _redisDatabase.StringGetAsync(key);
        }

        public async Task<RedisValue> SetValueAsync(string key, string value, TimeSpan? expiration = null)
        {
            return await _redisDatabase.StringSetAndGetAsync(key, value, expiration);
        }
    }
}
