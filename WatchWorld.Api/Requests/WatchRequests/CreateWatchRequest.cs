using WatchWorld.Domain.Entities;
using WatchWorld.Domain.Enums;

namespace WatchWorld.Api.Requests.WatchRequests
{
    public record CreateWatchRequest(        
    string name,
    string modelNumber,
    int caseSize,
    CaseShapeEnum caseShapeEnum,
    CaseMaterialEnum caseMaterialEnum,
    MovementTypeEnum movementTypeEnum,
    string style,
    decimal originalPrice,
    GenderEnum genderEnum,
    DateOnly releaseYear,
    List<BraceletTypeEnum> braceletTypeEnum,
    string description,
    List<HighResImage> images
    )
    {

    }
}   
