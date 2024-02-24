using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Infrastructure.Data.Contexts;

namespace Vendaval.Infrastructure.DependencyInjection
{
    public static class DbContextCollection
    {
        public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            var connStr = new ConnStr(configuration);

            services.AddDbContextPool<VendavalDbContext>(options =>
            {
                options.UseNpgsql(connStr.VendavalDb);
            });

            var redis = ConnectionMultiplexer.Connect(connStr.Redis);
            var redisDb = redis.GetDatabase();

            services.AddScoped(s => redisDb);
            return services;
        }
    }
}
