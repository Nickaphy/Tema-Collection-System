using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WatchWorld.Application.Ports.OutBound;
using WatchWorld.Infrastructure.Adapters;
using WatchWorld.Infrastructure.Database;

namespace WatchWorld.Infrastructure
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDatabaseServices(configuration);
            services.AddScoped<IWatchesRepository, SqlServerWatchRepository>();
            services.AddScoped<IUserRepository, SqlServerUserRepository>();
            services.AddScoped<IIndividualWatchRepository, SqlServerIndividualWatchRepository>();

            return services;
        }
    }
            
}
