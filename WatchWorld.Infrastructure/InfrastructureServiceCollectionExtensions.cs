using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WatchWorld.Application.Ports.OutBound;
using WatchWorld.Infrastructure.Adapters;
using WatchWorld.Application.Ports.OutBound.Services;
using WatchWorld.Infrastructure.Security;
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
            services.AddScoped<IListingRepository, SqlServerListingRepository>();
            services.AddScoped<IBorrowRepository, SqlServerBorrowRepository>();
            services.AddScoped<IUserRatingRepository, SqlServerUserRatingRepository>();
            services.AddScoped<IImageRepository, SqlServerHighResImageRepository>();
            services.AddScoped<IBrandRepository, SqlServerBrandRepository>();
            services.AddOptions<JwtSettings>()
                .Bind(configuration.GetSection(JwtSettings.SectionName))
                .Validate(settings => settings.Secret.Length >= 32,
                    "JwtSettings:Secret is missing or shorter than 32 characters. Check JWT_SECRET in .env")
                .ValidateOnStart();
            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
            
            return services;
        }
    }
            
}
