using WatchWorld.Domain.Entities;
using WatchWorld.Domain.Enums;

namespace WatchWorld.Application.Ports.InBound;

public interface IWatchesUseCase
{
    Task<Watches> CreateWatchAsync(string name, string modelNumber, int caseSize, CaseShapeEnum caseShapeEnum, CaseMaterialEnum caseMaterialEnum, MovementTypeEnum movementTypeEnum, string style, decimal originalPrice, GenderEnum genderEnum, DateOnly releaseYear, List<BraceletTypeEnum> braceletTypeEnum, string description, List<HighResImage> images, CancellationToken cancellationToken = default);
    Task<IEnumerable<Watches?>> GetAllAsync(CancellationToken ct = default);
}
