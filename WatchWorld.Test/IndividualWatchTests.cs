using WatchWorld.Domain.Entities;
using WatchWorld.Domain.Enums;
using Xunit;

namespace WatchWorld.Test;

public class IndividualWatchTests
{
    [Fact]
    public void Can_create_individual_watch_of_an_older_model()
    {
        var model = Watches.Create(
            "Test Watch", "12345", 41,
            CaseShapeEnum.Round, CaseMaterialEnum.RoseGold, MovementTypeEnum.Automatic,
            "Dress", 9000m, GenderEnum.Unisex, new DateOnly(2010, 1, 1),
            new List<BraceletTypeEnum> { BraceletTypeEnum.RoseGold },
            "test", new List<HighResImage>());

        var watch = IndividualWatch.Create(
            model, WearGradeEnum.FactoryNew, 3, "note", 5000m, new List<HighResImage>());

        Assert.NotNull(watch);
    }
}