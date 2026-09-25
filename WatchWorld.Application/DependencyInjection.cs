using Microsoft.Extensions.DependencyInjection;
using WatchWorld.Application.Ports.InBound;
using WatchWorld.Application.Services;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services)
    {
        //UseCase injecting into services
        // IBorrowUseCase, IImagesUseCase, IListingUseCase, IUserRatingUseCase are disabled:
        // their backing repositories (IBorrowRepository, IImageRepository, IListingRepository,
        // IUserRatingRepository) have no Infrastructure adapter yet, and ASP.NET's DI validation
        // (ValidateOnBuild, on by default in Development) crashes app startup if these are
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
