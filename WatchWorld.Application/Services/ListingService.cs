using FluentResults;
using WatchWorld.Application.Commands.ImageCommands;
using WatchWorld.Application.Commands.ListingCommands;
using WatchWorld.Application.Ports.InBound;
using WatchWorld.Application.Ports.OutBound;
using WatchWorld.Domain.Entities;
using WatchWorld.Domain.Service;

namespace WatchWorld.Application.Services;

public class ListingService : IListingUseCase
{
    private readonly IListingRepository _repository;
    private static readonly SemaphoreSlim _Lock = new(1, 1);

    public ListingService(IListingRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<Listing?>>> GetAllAsync(CancellationToken ct = default)
    {
        var listings = await _repository.GetAllAsync(ct);
        return Result.Ok(listings.Value);
    }

    public async Task<Result<Listing>> GetListingByIdAsync(Guid id, CancellationToken ct = default)
    {
        var listing = await _repository.GetListingByIdAsync(id, ct);
        if (listing.IsFailed || listing.Value == null)
        {
            return Result.Fail("Opstillingen blev ikke fundet.");
        }
        return Result.Ok(listing.Value);
    }

    public async Task<Result<Listing>> CreateListingAsync(CreateListingCommand command, CancellationToken ct = default)
    {
        var existingListings = await _repository.GetAllAsync(ct);

        await _Lock.WaitAsync();
        try
        {
            try
            {

                var listing = Listing.Create(
                    borrowableWatch: command.borrowableWatchId,
                    pricePerDay: command.pricePerDay
                );
                await _repository.CreateListingAsync(listing);
                return Result.Ok(listing);
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

    public async Task<Result> DeleteListingAsync(DeleteListingCommand deleteListingCommand, CancellationToken ct = default)
    {
        var existingListing = await _repository.GetListingByIdAsync(deleteListingCommand.id, ct);
        if (existingListing == null)
        {
            return Result.Fail("Opstillingen blev ikke fundet.");
        }
        await _repository.DeleteListingAsync(deleteListingCommand.id, ct);
        return Result.Ok();
    }
}
