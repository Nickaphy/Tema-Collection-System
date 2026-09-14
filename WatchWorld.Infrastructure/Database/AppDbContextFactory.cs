using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WatchWorld.Infrastructure.Database
{
    // Used only by `dotnet ef` at design time (e.g. `dotnet ef migrations add`).
    // Scaffolding a migration doesn't require a live DB connection, so this builds
    // a connection string pointed at the docker-compose SQL Server exposed on
    // localhost:1433 instead of depending on appsettings.json (which doesn't exist)
    // or a full ASP.NET host.
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                var saPassword = Environment.GetEnvironmentVariable("SA_PASSWORD")
                    ?? throw new InvalidOperationException(
                        "SA_PASSWORD is not set. Run `export SA_PASSWORD=$(grep SA_PASSWORD .env | cut -d= -f2 | tr -d ' ')` " +
                        "from the repo root before running `dotnet ef`, or set ConnectionStrings__DefaultConnection directly.");

                connectionString = $"Server=localhost,1433;Database=WatchWorld;User Id=sa;Password={saPassword};TrustServerCertificate=True;";
            }

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
