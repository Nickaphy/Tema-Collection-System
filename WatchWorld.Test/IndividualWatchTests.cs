using WatchWorld.Domain.Entities;
using WatchWorld.Domain.Enums;
using Xunit;

namespace WatchWorld.Test;

public class IndividualWatchTests
{
    [Fact]
    public void Can_create_individual_watch_of_an_older_model()
    {
        var dummyBrand = Brand.Create("Dummy Brand", 2020, "Dummy Country", "Dummy Group", "Dummy Founder", "Dummy Founder Note", "Dummy Origin Note", "Dummy Brand Note", "Dummy Style Note", "Dummy Logo URL", "Dummy Website URL", true);
        var dummyUser = User.Create("Dummy", "User", "61737544", "dummyuser@example.com", "Dummy User Note", "hej", "hej", "HejMedDig!=Din2213212Bitch", new List<UserRating>());

        var model = Watches.Create(
            "Test Watch", dummyBrand.Id, "12345", 41,
            CaseShapeEnum.Round, CaseMaterialEnum.RoseGold, MovementTypeEnum.Automatic,
            "Dress", 9000m, GenderEnum.Unisex, new DateOnly(2010, 1, 1),
            new List<BraceletTypeEnum> { BraceletTypeEnum.RoseGold },
            "test", new List<HighResImage>());

        var watch = IndividualWatch.Create(
            model, dummyUser.Id, WearGradeEnum.FactoryNew, 3, "note", 5000m, new List<HighResImage>());

        Assert.NotNull(watch);
    }
}