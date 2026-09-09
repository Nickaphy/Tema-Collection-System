using WatchWorld.Application.Ports.InBound;
using WatchWorld.Application.Ports.OutBound;
using WatchWorld.Domain.Entities;
using WatchWorld.Domain.Enums;

namespace WatchWorld.Application.Services;

public class WatchesService : IWatchesUseCase
{
    private readonly IWatchesRepository _repository;

    public WatchesService(IWatchesRepository repository)
    {
        _repository = repository;
    }

    public Task<Watches> CreateWatchAsync(string name, string modelNumber, int caseSize, CaseShapeEnum caseShapeEnum, CaseMaterialEnum caseMaterialEnum, MovementTypeEnum movementTypeEnum, string style, decimal originalPrice, GenderEnum genderEnum, DateOnly releaseYear, List<BraceletTypeEnum> braceletTypeEnum, string description, List<HighResImage> images, CancellationToken cancellationToken = default)
    {
        // TODO: IWatchesRepository has no save/add method yet, so this isn't persisted anywhere.
        var watch = Watches.Create(name, modelNumber, caseSize, caseShapeEnum, caseMaterialEnum, movementTypeEnum, style, originalPrice, genderEnum, releaseYear, braceletTypeEnum, description, images);
        return Task.FromResult(watch);
    }

    public async Task<IEnumerable<Watches?>> GetAllAsync(CancellationToken ct = default)
    {
        return await _repository.GetAllAsync(ct);
    }
}
