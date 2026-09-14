using FysioEnterprise.Presentation.Service.Helpers;
using WatchWorld.BlazorUI.Helper;

namespace WatchWorld.BlazorUI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUIServices(
            this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<LogInContext>();
            services.AddScoped<NotificationHelper>();
            return services;
        }
    }
}
