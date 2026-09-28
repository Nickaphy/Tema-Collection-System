using Microsoft.EntityFrameworkCore;
using WatchWorld.Domain.Entities;
using WatchWorld.Domain.Enums;
using WatchWorld.Infrastructure.Database;
using WatchWorld.Infrastructure.Database.Seed;

public static class WatchSeeder
{
    private static readonly Dictionary<string, string> ImageDictionary = new(StringComparer.OrdinalIgnoreCase)
    {
        // Audemars Piguet Models
        { "26393OR.OO.A056KB.01", "https://upload.wikimedia.org/wikipedia/commons/2/23/Audemars_Piguet_Royal_Oak_in_oro_e_tantalio%2C_fine_anni_%2780-primi_%2790.jpg" },
        { "26395NR.OO.D002KB.01", "https://upload.wikimedia.org/wikipedia/commons/6/60/Audemars_Piguet_skeleton_watch.jpg" },
        { "26441OR.OO.D405CR.01", "https://upload.wikimedia.org/wikipedia/commons/3/30/Audemars_Piguet_Royal_Oak_Offshore_Chronograph.jpg" },
        { "6393OR.OO.A056KB.01",  "https://upload.wikimedia.org/wikipedia/commons/2/23/Audemars_Piguet_Royal_Oak_in_oro_e_tantalio%2C_fine_anni_%2780-primi_%2790.jpg" }, // Used same as above due to typo in model nr
        { "77410OR.OO.A623CR.01", "https://upload.wikimedia.org/wikipedia/commons/thumb/c/cd/Audemars_Piguet_watch.jpg/1024px-Audemars_Piguet_watch.jpg" },
        { "77410OR.ZZ.D343CR.01", "https://upload.wikimedia.org/wikipedia/commons/8/89/Audemars_Piguet_-_Royal_Oak_Concept_Laptimer_Michael_Schumacher.jpg" },
        
        // Breitling Models
        { "A17328101B1X1", "https://upload.wikimedia.org/wikipedia/commons/8/87/Breitling_Navitimer_01.jpg" },
        { "A173283A1I1X1", "https://upload.wikimedia.org/wikipedia/commons/d/df/Breitling_Superocean_Steelfish_X-Plus.jpg" },
        { "A24315101C1X2", "https://upload.wikimedia.org/wikipedia/commons/c/cd/Breitling_Chronomat_B01.jpg" },
        { "AB0147101L1A1", "https://upload.wikimedia.org/wikipedia/commons/e/ea/Breitling_Emergency_2.jpg" }
    };

    private static readonly string[] DefaultFallbackImages = new[]
    {
        "https://upload.wikimedia.org/wikipedia/commons/8/87/Breitling_Navitimer_01.jpg",
        "https://upload.wikimedia.org/wikipedia/commons/2/23/Audemars_Piguet_Royal_Oak_in_oro_e_tantalio%2C_fine_anni_%2780-primi_%2790.jpg",
        "https://upload.wikimedia.org/wikipedia/commons/3/30/Audemars_Piguet_Royal_Oak_Offshore_Chronograph.jpg"
    };

    // Seed watches into database, basic = 20, full = 150. (local, mother)
    public static async Task SeedWatchesAsync(AppDbContext context, bool useFullCatalog)
    {
        if (await context.Watchlist.AnyAsync())
            return;

        var seedSet = useFullCatalog ? WatchSeedData.All : WatchSeedData.Basic;
        int fallbackIndex = 0;

        foreach (var seed in seedSet)
        {
            var watchImages = new List<HighResImage>();

            if (ImageDictionary.TryGetValue(seed.ModelNumber, out string matchedUrl))
            {
                watchImages.Add(HighResImage.Create(matchedUrl, 1000, 1000));
            }
            else
            {
                string fallbackUrl = DefaultFallbackImages[fallbackIndex % DefaultFallbackImages.Length];
                watchImages.Add(HighResImage.Create(fallbackUrl, 1000, 1000));
                fallbackIndex++;
            }

            var watch = Watches.Create(
                seed.Name,
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
