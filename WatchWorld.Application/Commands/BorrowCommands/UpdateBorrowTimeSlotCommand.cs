using WatchWorld.Domain.Enums;
using WatchWorld.Domain.ValueObjects;

namespace WatchWorld.Application.Commands.BorrowCommands
{
    public record UpdateBorrowTimeSlotCommand(
        Guid id,
        TimeSlot borrowTimeSlot)
    {
    }
}
