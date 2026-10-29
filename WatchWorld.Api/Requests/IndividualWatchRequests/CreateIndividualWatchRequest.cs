
using WatchWorld.Api.Requests.WatchRequests;
using WatchWorld.Domain.Enums;

namespace WatchWorld.Api.Requests.IndividualWatchRequests
{
    public record CreateIndividualWatchRequest(
    Guid specificWatchId,
    Guid userId,
    WearGradeEnum wearGrade,
    int age,
    string note,
    decimal estimatedValue,
    List<CreateImageRequest> picture
    )
    {

    }
}
