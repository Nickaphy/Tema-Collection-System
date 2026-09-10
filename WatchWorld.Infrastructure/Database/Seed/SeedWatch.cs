using WatchWorld.Domain.Enums;

namespace WatchWorld.Infrastructure.Database.Seed;

// Plain data shape for one seed row - kept separate from the Watches
// aggregate so this file carries no business logic, only data.
public record SeedWatch(
    string Name,
    string ModelNumber,
    int CaseSize,
    CaseShapeEnum CaseShape,
    CaseMaterialEnum CaseMaterial,
    MovementTypeEnum Movement,
    string Style,
    decimal OriginalPrice,
    GenderEnum Gender,
    int ReleaseYear,
    BraceletTypeEnum Bracelet,
    string Description);
