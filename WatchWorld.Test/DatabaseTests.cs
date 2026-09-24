using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WatchWorld.Infrastructure.Database;
using Xunit;

namespace WatchWorld.Test
{
    public class ResolveConnectionStringTets
    {
        private const string Local = "Server=local-host;Database=WatchWorld;User Id=sa;Password=x;TrustServerCertificate=True;";


        // Kører AddDatabaseServices med en falsk konfiguration og returnerer den valgte connection string
        private static string? ResolvedConnectionString(string? mother)
        {
            var settings = new Dictionary<string, string?> { ["ConnectionStrings:Local"] = Local };

            if (mother is not null)
                settings["ConnectionStrings:Mother"] = mother;

            var config = new ConfigurationBuilder().AddInMemoryCollection(settings).Build();

            var services = new ServiceCollection();
            services.AddDatabaseServices(config);

            using var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            return db.Database.GetConnectionString();
        }

        //Test 1: Hvis mother mangler bruges Local.
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Uses_Local_when_Mother_is_Missing(string? mother)
        {
            var result = ResolvedConnectionString(mother);

            Assert.Equal(Local, result);
        }

        //Test 2: Mother er der, men kan ikke tilgåes, så bruges Local
        [Fact]
        public void Falls_back_to_local_when_mother_is_unreachable()
        {
            var unreachableMother = "Server=127.0.0.1,1;Database=WatchWorld;User Id=sa;Password=x;TrustServerCertificate=True;Connect Timeout=2;";

            var result = ResolvedConnectionString(unreachableMother);

            Assert.Equal(Local, result);
        }

        //Test 3: Mother bliver brugt og kan nås
        [Fact(Skip = "Der kræves en kørende SQL Server")]
        public void Uses_Mother_when_reachable()
        {
            var password = Environment.GetEnvironmentVariable("SA_PASSWORD")
                ?? throw new InvalidOperationException("Sæt variablen for SA_PASSWORD.");

            var reachableMother = $"Server=localhost,1433;Database=master;User Id=sa;Password={password};TrustServerCertificate=True;";

            var result = ResolvedConnectionString(reachableMother);

            Assert.Equal(reachableMother, result);
        }

    }
}
