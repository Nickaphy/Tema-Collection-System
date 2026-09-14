using WatchWorld.Domain.Enums;
using WatchWorld.Domain.ValueObjects;

namespace WatchWorld.Application.Commands.BorrowCommands
{
    public record CreateBorrowCommand(
        Guid borrowedByUserId,
        Guid borrowedFromUserId,
        TimeSlot borrowTimeSlot,
        BorrowStatus status)
    {

    }
}
