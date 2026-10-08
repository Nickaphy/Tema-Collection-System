using Microsoft.Data.SqlClient;
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


        // Helper function (replacing Asserts when comparing locals and results)
        private static void AssertSameConnectionString(string expected, string? actual)
        {
            Assert.NotNull(actual);

            var expectedBuilder = new SqlConnectionStringBuilder(expected);
            var actualBuilder = new SqlConnectionStringBuilder(actual);

            Assert.Equal(expectedBuilder.DataSource, actualBuilder.DataSource);
            Assert.Equal(expectedBuilder.InitialCatalog, actualBuilder.InitialCatalog);
            Assert.Equal(expectedBuilder.UserID, actualBuilder.UserID);
            Assert.Equal(expectedBuilder.Password, actualBuilder.Password);
            Assert.Equal(expectedBuilder.TrustServerCertificate, actualBuilder.TrustServerCertificate);
        }




        // Kører AddDatabaseServices med en falsk konfiguration og returnerer den valgte connection string (why danish lil bro?)
        private static string? ResolvedConnectionString(string? mother)
        {
            var settings = new Dictionary<string, string?> { ["ConnectionStrings:Local"] = Local };

            if (mother is not null)
                settings["ConnectionStrings:Mother"] = mother;


            // DI implementation in test
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

            AssertSameConnectionString(Local, result);
        }

        //Test 2: Mother er der, men kan ikke tilgåes, så bruges Local
        [Fact]
        public void Falls_back_to_local_when_mother_is_unreachable()
        {
            var unreachableMother = "Server=127.0.0.1,1;Database=WatchWorld;User Id=sa;Password=x;TrustServerCertificate=True;Connect Timeout=2;";

            var result = ResolvedConnectionString(unreachableMother);

            AssertSameConnectionString(Local, result);
        }

        
        /*
        Github Actions, does not have our .env file, and will therefore fail our CI check :C (not good)
        We will therefore attempt to get password via environment variable down below instead of calling this function.
        But we will also run this local test, but just take care of the CI aswell :D
        */

        private static string? ReadSaPasswordFromEnvFile()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);

            while (dir != null)
            {
                var envFile = Path.Combine(dir.FullName, ".env");
                if (File.Exists(envFile))
                {
                    foreach (var line in File.ReadAllLines(envFile))
                    {
                        if (line.StartsWith("SA_PASSWORD="))
                            return line.Substring("SA_PASSWORD=".Length).Trim().Trim('"', '\'');
                    }
                }
                dir = dir.Parent;
            }

            return null;
        }
        

        //Her bliver mother brugt og kan nås ved hjælp af docker compose up
        [Trait("Category", "Integration")]
        [Fact]
        public void Uses_Mother_when_reachable()
        {
            /* 
            Replaces the commented out function above, github actions does not have access to .env file, it has its own environmentvarialbe secret.
            Github actions already exposes `SA_PASSWORD: ${{ secrets.CI_SA_PASSWORD }}`. 
            Also running the local readEnv test.
            */
           var password =
            Environment.GetEnvironmentVariable("SA_PASSWORD")
            ?? ReadSaPasswordFromEnvFile() 
            ?? throw new InvalidOperationException("SA_PASSWORD was not found.");


            var reachableMother = $"Server=localhost,1433;Database=master;User Id=sa;Password={password};TrustServerCertificate=True;Connect Timeout=3;";

            var result = ResolvedConnectionString(reachableMother);

            AssertSameConnectionString(reachableMother, result);
        }
    }
}