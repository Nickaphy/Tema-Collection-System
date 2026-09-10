
using WatchWorld.Domain.Entities;

namespace WatchWorld.Api.Requests.ListingRequests
{
    public record CreateListingRequest (IndividualWatch borrowableWatchId, decimal pricePerDay)
    {

    }
}
