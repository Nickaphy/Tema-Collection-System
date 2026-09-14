using WatchWorld.Domain.Enums;
using WatchWorld.Domain.ValueObjects;

namespace WatchWorld.Api.Requests.BorrowRequests
{
    public record UpdateBorrowTimeSlotRequest(
        Guid borrowId,
        TimeSlot borrowTimeSlot)
    {

    }
}
