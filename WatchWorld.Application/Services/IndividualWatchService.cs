using FluentResults;
using WatchWorld.Application.Commands.IndividualWatchCommands;
using WatchWorld.Application.Ports.InBound;
using WatchWorld.Application.Ports.OutBound;
using WatchWorld.Domain.Entities;
using WatchWorld.Domain.Service;

namespace WatchWorld.Application.Services;

public class IndividualWatchService : IIndividualWatchUseCase
{
    private readonly IWatchesRepository _watchRepository;
    private readonly IIndividualWatchRepository _individualWatchRepository;
    private static readonly SemaphoreSlim _Lock = new(1, 1);

    public IndividualWatchService(IWatchesRepository repository, IIndividualWatchRepository individualWatchRepository)
    {
        _watchRepository = repository;
        _individualWatchRepository = individualWatchRepository;
    }

    public async Task<Result<IEnumerable<IndividualWatch?>>> GetAllAsync(CancellationToken ct = default)
    {
        var watches = await _individualWatchRepository.GetAllAsync(ct);
        return Result.Ok(watches.Value);
    }

    public async Task<Result<IndividualWatch>> CreateIndividualWatchAsync(CreateIndividualWatchCommand command, CancellationToken ct = default)
    {
        await _Lock.WaitAsync();
        try
        {
            var existingWatch = await _watchRepository.GetWatchByIdAsync(command.specificWatchId, ct);
            try
            {

                var watch = IndividualWatch.Create(
                    specificWatch: existingWatch.Value,
                    wearGrade: command.wearGrade,
                    age: command.age,
                    note: command.note,
                    estimatedValue: command.estimatedValue,
                    picture: command.picture

                );
                await _individualWatchRepository.CreateWatchAsync(watch, ct);
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

    public async Task<Result<IndividualWatch>> GetIndividualWatchByIdAsync(Guid id, CancellationToken ct = default)
    {
        var existingIndividualWatch = await _individualWatchRepository.GetWatchByIdAsync(id, ct);
        if (existingIndividualWatch.IsFailed || existingIndividualWatch.Value == null)
        {
            return Result.Fail("Uret blev ikke fundet.");
        }
        return Result.Ok(existingIndividualWatch.Value);
    }

    public async Task<Result> DeleteIndividualWatchAsync(DeleteIndividualWatchCommand deleteIndividualWatchCommand, CancellationToken ct = default)
    {
        var existingIndividualWatch = await _individualWatchRepository.GetWatchByIdAsync(deleteIndividualWatchCommand.id, ct);
        if (existingIndividualWatch == null)
        {
            return Result.Fail("Uret blev ikke fundet og kan dermed ikke slettes");
        }
        await _individualWatchRepository.DeleteWatchAsync(deleteIndividualWatchCommand.id, ct);
        return Result.Ok();
    }
}