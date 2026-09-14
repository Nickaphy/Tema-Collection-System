using Microsoft.Extensions.DependencyInjection;
using WatchWorld.Application.Ports.InBound;
using WatchWorld.Application.Services;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services)
    {
        //UseCase injecting into services
        services.AddScoped<IBorrowUseCase, BorrowService>();
        services.AddScoped<IImagesUseCase, ImageService>();
        services.AddScoped<IIndividualWatchUseCase, IndividualWatchService>();
        services.AddScoped<IListingUseCase, ListingService>();
        services.AddScoped<IUserRatingUseCase, UserRatingService>();
        services.AddScoped<IUserUseCase, UserService>();
        services.AddScoped<IWatchesUseCase, WatchesService>();

        return services;
    }
}
