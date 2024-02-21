using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Application.Services;
using Vendaval.Application.Services.Interfaces;

namespace Vendaval.Application.DependencyInjection
{
    public static class ServiceCollection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserViewModelSerivce, UserViewModelService>();
            return services;
        }
    }
}
