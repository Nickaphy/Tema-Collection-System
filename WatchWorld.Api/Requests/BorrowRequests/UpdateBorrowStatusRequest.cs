using WatchWorld.Domain.Enums;

namespace WatchWorld.Api.Requests.BorrowRequests
{
    public record UpdateBorrowStatusRequest(
        Guid borrowId,
        BorrowStatus status,
        BorrowStatus targetStatus)
    {

    }
}
