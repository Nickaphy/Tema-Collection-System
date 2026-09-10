using FluentResults;
using WatchWorld.Application.Commands.WatchesCommands;
using WatchWorld.Application.Ports.InBound;
using WatchWorld.Application.Ports.OutBound;
using WatchWorld.Domain.Entities;
using WatchWorld.Domain.Service;

namespace WatchWorld.Application.Services;

public class WatchesService : IWatchesUseCase
{
    private readonly IWatchesRepository _repository;
    private static readonly SemaphoreSlim _Lock = new(1, 1);

    public WatchesService(IWatchesRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<Watches?>>> GetAllAsync(CancellationToken ct = default)
    {
        var watches = await _repository.GetAllAsync(ct);
        return Result.Ok(watches);
    }

    public async Task<Result<Watches>> CreateWatchAsync(CreateWatchCommand command, CancellationToken ct = default)
    {
        var existingWatches = await _repository.GetAllAsync(ct);

        await _Lock.WaitAsync();
        try
        {
            try
            {

            var watch = Watches.Create(
                name: command.name,
                modelNumber: command.modelNumber,
                caseSize: command.caseSize,
                caseShapeEnum: command.caseShapeEnum,
                caseMaterialEnum: command.caseMaterialEnum,
                movementTypeEnum: command.movementTypeEnum,
                style: command.style,
                originalPrice: command.originalPrice,
                genderEnum: command.genderEnum,
                releaseYear: command.releaseYear,
                braceletTypeEnum: command.braceletTypeEnum,
                description: command.description,
                images: command.images
            );
                await _repository.CreateWatchAsync(watch);
                return Result.Ok(watch);
            }
            catch (DomainException ex)
            {
                return ex switch
                {
                    UserInvalidInputException => Result.Fail("Et input var ikke korrekt. " + ex.Message),
                    ValidationException => Result.Fail("Der er sket en valideringsfejl. " + ex.Message),
                    _ => Result.Fail("Der er sket en uforventet fejl " + ex.Message) // Fallback catch-all for base DomainException
                };
            }
            catch (Exception ex) // Catch-all for any other unexpected exceptions (typically SQL or Infrastructure exceptions)
            {
                System.Diagnostics.Debug.WriteLine($"Infrastructure Failure: {ex.Message}");
                return Result.Fail("An unexpected system error occurred." + ex.Message);
            }
        }
        finally
        {
            _Lock.Release();
        }
    }
}
