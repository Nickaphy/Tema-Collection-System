using WatchWorld.Domain.Enums;
using WatchWorld.Domain.ValueObjects;

namespace WatchWorld.Api.Requests.BorrowRequests
{
    public record CreateBorrowRequest(
        Guid borrowedByUserId,
        Guid borrowedFromUserId,
        TimeSlot borrowTimeSlot,
        BorrowStatus status)
    {
        
    }
}
