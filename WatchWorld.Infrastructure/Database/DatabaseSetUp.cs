using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WatchWorld.Infrastructure.Database;
using WatchWorld.Infrastructure.Database.Seed;

public static class DatabaseSetup
{
    // Registers AppDbContext with SQL Server into the ServiceCollection.
    public static IServiceCollection AddDatabaseServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
                               ?? throw new InvalidOperationException("Missing ConnectionStrings:DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString, sqlOptions =>
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 10,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null)));

        return services;
    }
    public static async Task InitializeDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await context.Database.MigrateAsync();

        // Both seeders are static (they check for existing rows before inserting),
        // so it's safe to run them on every startup. WatchSeeder must run first -
        // DbSeeder's borrow/listing data depends on watches already existing.
        await WatchSeeder.SeedWatchesAsync(context);
        await DbSeeder.SeedAsync(context);
    }
}
