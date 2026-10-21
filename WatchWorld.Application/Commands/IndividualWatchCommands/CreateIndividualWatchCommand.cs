using WatchWorld.Domain.Entities;
using WatchWorld.Domain.Enums;

namespace WatchWorld.Application.Commands.IndividualWatchCommands
{
    public record CreateIndividualWatchCommand(
    Guid specificWatchId,
    Guid userId,
    WearGradeEnum wearGrade,
    int age,
    string note,
    decimal estimatedValue,
    List<HighResImage> picture
    )
    {

    }
}
