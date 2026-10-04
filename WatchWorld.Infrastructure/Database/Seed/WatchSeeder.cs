using Microsoft.EntityFrameworkCore;
using WatchWorld.Domain.Entities;
using WatchWorld.Domain.Enums;
using WatchWorld.Infrastructure.Database;
using WatchWorld.Infrastructure.Database.Seed;

public static class WatchSeeder
{
    private const string BaseUrl = "https://localhost:64369/images/seed";
    private const string PlaceholderImage = $"{BaseUrl}/OnTheWay.png";

    // The 10 model numbers you manually saved in wwwroot/images/seed/
    private static readonly HashSet<string> LocalModelImages = new(StringComparer.OrdinalIgnoreCase)
    {
        "26393OR.OO.A056KB.01",
        "26395NR.OO.D002KB.01",
        "26441OR.OO.D405CR.01",
        "6393OR.OO.A056KB.01",
        "77410OR.OO.A623CR.01",
        "77410OR.ZZ.D343CR.01",
        "A17328101B1X1",
        "A173283A1I1X1",
        "A24315101C1X2",
        "AB0147101L1A1"
    };

    // Seed watches into database, basic = 20, full = 150. (local, mother)
    public static async Task SeedWatchesAsync(AppDbContext context, bool useFullCatalog)
    {
        // 1. BACKDOOR: Wipe existing records in dependency order
        if (await context.Watchlist.AnyAsync())
        {
            await context.Database.ExecuteSqlRawAsync("DELETE FROM IndividualWatches");
            await context.Database.ExecuteSqlRawAsync("DELETE FROM HighResImages");
            await context.Database.ExecuteSqlRawAsync("DELETE FROM Watchlist");
        }

        // 2. Fetch all seeded brands from the database into memory
        var allBrands = await context.Brands.ToListAsync();

        if (!allBrands.Any())
        {
            throw new InvalidOperationException("Critical error, no brands found. Please ensure Brands are seeded before Watches.");
        }

        var seedSet = useFullCatalog ? WatchSeedData.All : WatchSeedData.Basic;

        foreach (var seed in seedSet)
        {
            var matchedBrand = allBrands
                .FirstOrDefault(b => b.Name.Equals(seed.Brand, StringComparison.OrdinalIgnoreCase));

            if (matchedBrand == null)
            {
                Console.WriteLine($"[Warning] Could not identify a brand for watch: {seed.Name}. Skipping.");
                continue;
            }

            string imageUrl = LocalModelImages.Contains(seed.ModelNumber)
                ? $"{BaseUrl}/{seed.ModelNumber}.jpg"
                : PlaceholderImage;

            var watchImages = new List<HighResImage>
        {
            HighResImage.Create(imageUrl, 1000, 1000)
        };

            var watch = Watches.Create(
                seed.Name,
                matchedBrand.Id, // Passing the relational ID required by the database
                seed.ModelNumber,
                seed.CaseSize,
                seed.CaseShape,
                seed.CaseMaterial,
                seed.Movement,
                seed.Style,
                seed.OriginalPrice,
                seed.Gender,
                new DateOnly(seed.ReleaseYear, 1, 1),
                new List<BraceletTypeEnum> { seed.Bracelet },
                seed.Description,
                watchImages);

            context.Watchlist.Add(watch);
        }

        await context.SaveChangesAsync();
    }
}
