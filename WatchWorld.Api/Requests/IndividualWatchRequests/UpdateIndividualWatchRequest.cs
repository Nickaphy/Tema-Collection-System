using WatchWorld.Api.Requests.WatchRequests;
using WatchWorld.Domain.Enums;

namespace WatchWorld.Api.Requests.IndividualWatchRequests
{
    public record UpdateIndividualWatchRequest(
    Guid individualWatchId,
    Guid specificWatchId,
    WearGradeEnum wearGrade,
    int age,
    string note,
    decimal estimatedValue,
    List<CreateImageRequest> picture
    )
    {

    }
}
