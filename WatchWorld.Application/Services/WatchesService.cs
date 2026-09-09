using WatchWorld.Application.Ports.InBound;
using WatchWorld.Application.Ports.OutBound;
using WatchWorld.Domain;

namespace WatchWorld.Application.Services;

public class WatchesService : IWatchesUseCase
{
    private readonly IWatchesRepository _repository;

    public WatchesService(IWatchesRepository repository)
    {
        _repository = repository;
    }
}
