using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WatchWorld.Infrastructure.Database;
using WatchWorld.Infrastructure.Database.Seed;

// DATABASE MAGIC 
// WHICHEVER CONNECTION STRING ENDS UP RETURNED FROM RESOLVECONNECTIONSTRING IS THE
// ONE THAT WILL BE PASSED TO DBCONTEXT AND USED!

public static class DatabaseSetup
{
    // Registers AppDbContext with SQL Server into the ServiceCollection.
    public static IServiceCollection AddDatabaseServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Get the connection strings for the Mother and Local databases
        var motherConnectionString = configuration.GetConnectionString("Mother");
        var localConnectionString = configuration.GetConnectionString("Local")
                               ?? throw new InvalidOperationException("Missing ConnectionStrings:Local");

        var connectionString = ResolveConnectionString(motherConnectionString, localConnectionString);

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString, sqlOptions =>
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 10,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null)));

        return services;
    }


    private static string ResolveConnectionString(string? motherConnectionString, string localConnectionString)
    {
        // If the Mother connection string is not set, use the Local connection string
        if (string.IsNullOrWhiteSpace(motherConnectionString))
        {
            Console.WriteLine("[Database] No Mother connection string configured - using Local.");
            return localConnectionString;
        }

        // Try to connect to the Mother database
        try
        {
            using var connection = new SqlConnection(motherConnectionString);
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3)); // Timeout after 3 seconds
            connection.OpenAsync(cts.Token).GetAwaiter().GetResult(); // Open the connection asynchronously
            Console.WriteLine("[Database] Connected to Mother (shared LAN database).");
            return motherConnectionString;
        }
        // If the connection fails, use the Local connection string
        catch (Exception ex)
        {
            Console.WriteLine($"[Database] Mother unreachable ({ex.GetType().Name}: {ex.Message}) - falling back to Local.");
            return localConnectionString;
        }
    }
    public static async Task InitializeDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        // =======================================================
        // 1. WIPE LOCAL DATABASE (SAFE RE-SEEDING)
        // =======================================================
        var currentConnectionString = context.Database.GetConnectionString();
        var localConnectionString = configuration.GetConnectionString("Local");

        // Parse both strings to ignore EF Core's runtime formatting changes
        var currentBuilder = new SqlConnectionStringBuilder(currentConnectionString);
        var localBuilder = new SqlConnectionStringBuilder(localConnectionString);

        // Check if both the Server (DataSource) and Database (InitialCatalog) match
        bool isLocal = currentBuilder.DataSource.Equals(localBuilder.DataSource, StringComparison.OrdinalIgnoreCase) &&
                       currentBuilder.InitialCatalog.Equals(localBuilder.InitialCatalog, StringComparison.OrdinalIgnoreCase);

        if (isLocal)
        {
            Console.WriteLine("[Database] Local DB detected. Wiping database for fresh seed...");
            await context.Database.EnsureDeletedAsync();
        }
        else
        {
            Console.WriteLine("[Database] Mother DB detected. Skipping wipe to protect team data.");
        }

        // =======================================================
        // 2. MIGRATE & SEED
        // =======================================================
        Console.WriteLine("[Database] Applying migrations...");
        await context.Database.MigrateAsync();

        // Try to parse the SeedFullCatalog configuration flag
        _ = bool.TryParse(configuration["SeedFullCatalog"], out var useFullCatalog);

        await SeedBrands.SeedBrandsAsync(context);

        // Seed the watches into the database
        Console.WriteLine("[Database] Seeding Watches...");
        await WatchSeeder.SeedWatchesAsync(context, useFullCatalog);

        Console.WriteLine("[Database] Seeding remaining data...");
        await DbSeeder.SeedAsync(context);
    }
}