using WatchWorld.Domain.Enums;

namespace WatchWorld.Application.Commands.BorrowCommands
{
    public record UpdateBorrowStatusCommand(
        Guid borrowId,
        BorrowStatus status,
        BorrowStatus targetStatus)
    {
    }

}
