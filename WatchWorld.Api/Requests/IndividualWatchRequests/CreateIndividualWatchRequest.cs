using WatchWorld.Domain.Entities;
using WatchWorld.Domain.Enums;

namespace WatchWorld.Api.Requests.IndividualWatchRequests
{
    public record CreateIndividualWatchRequest(
    Guid specificWatchId,
    WearGradeEnum wearGrade,
    int age,
    string note,
    decimal estimatedValue,
    List<HighResImage> picture
    )
    {

    }
}
