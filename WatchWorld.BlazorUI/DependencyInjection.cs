using Radzen;
using WatchWorld.BlazorUI.Helpers;

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
            return services;
        }
    }
}
