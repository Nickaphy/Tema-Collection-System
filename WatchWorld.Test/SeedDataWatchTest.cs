using WatchWorld.Domain.Entities;
using WatchWorld.Domain.Enums;
using WatchWorld.Infrastructure.Database.Seed;
using Xunit;

namespace WatchWorld.Test;

public class SeedDataTests
{
    [Fact]
    public void Basic_is_the_first_20_watches_of_All()
    {
        Assert.Equal(20, WatchSeedData.Basic.Count);
        Assert.True(WatchSeedData.Basic.SequenceEqual(WatchSeedData.All.Take(20)));
    }

    [Fact]
    public void ModelNumbers_are_unique()
    {
        var unique = WatchSeedData.All.Select(s => s.ModelNumber).Distinct().Count();

        Assert.Equal(WatchSeedData.All.Count, unique);
    }

    [Fact]
    public void All_seed_watches_pass_domain_validation()
    {
        foreach (var seed in WatchSeedData.All)
        {
            var ex = Record.Exception(() => Watches.Create(
                seed.Name, seed.ModelNumber, seed.CaseSize, seed.CaseShape, seed.CaseMaterial,
                seed.Movement, seed.Style, seed.OriginalPrice, seed.Gender,
                new DateOnly(seed.ReleaseYear, 1, 1),
                new List<BraceletTypeEnum> { seed.Bracelet },
                seed.Description, new List<HighResImage>()));

            Assert.True(ex is null, $"{seed.ModelNumber} fejlede: {ex?.Message}");
        }
    }
}