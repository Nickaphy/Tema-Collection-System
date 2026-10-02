using Radzen;
using WatchWorld.BlazorUI.Helpers;
using WatchWorld.BlazorUI.Services;

namespace WatchWorld.BlazorUI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUIServices(
            this IServiceCollection services, IConfiguration config)
        {
            services.AddRadzenComponents();
            services.AddScoped<LogInContext>();
            services.AddScoped<NotificationHelper>();
            services.AddScoped<CurrentUserState>();
            return services;
        }
    }
}
