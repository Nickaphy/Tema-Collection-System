using WatchWorld.Domain.Entities;

namespace WatchWorld.Application.Commands.ListingCommands
{
    public record CreateListingCommand(IndividualWatch borrowableWatchId, decimal pricePerDay)
    {

    }
}
