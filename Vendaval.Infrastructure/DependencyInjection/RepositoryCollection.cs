using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Infrastructure.Data.Repositories.EFRepositories;
using Vendaval.Infrastructure.Data.Repositories.EFRepositories.Interfaces;
using Vendaval.Infrastructure.Data.Repositories.RedisRepositories;
using Vendaval.Infrastructure.Data.Repositories.RedisRepositories.Interfaces;

namespace Vendaval.Infrastructure.DependencyInjection
{
    public static class RepositoryCollection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRedisRepository,RedisRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductAvaliationRepository, ProductAvaliationRepository>();
            return services;
        }
    }
}
