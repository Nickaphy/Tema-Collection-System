using FluentResults;
using WatchWorld.Application.Commands.BorrowCommands;
using WatchWorld.Application.Ports.InBound;
using WatchWorld.Application.Ports.OutBound;
using WatchWorld.Domain.Entities;
using WatchWorld.Domain.Enums;
using WatchWorld.Domain.Service;

namespace WatchWorld.Application.Services;

public class BorrowService : IBorrowUseCase
{
    private readonly IBorrowRepository _borrowRepository;
    private static readonly SemaphoreSlim _Lock = new(1, 1);

    public BorrowService(IBorrowRepository borrowRepository)
    {
        _borrowRepository = borrowRepository;
    }

    public async Task<Result<IEnumerable<Borrow?>>> GetAllAsync(CancellationToken ct = default)
    {
        var borrows = await _borrowRepository.GetAllAsync(ct);
        return Result.Ok(borrows.Value);
    }

    public async Task<Result<Borrow>> GetBorrowByIdAsync(Guid id, CancellationToken ct = default)
    {
        try
        {
            var borrow = await _borrowRepository.GetBorrowByIdAsync(id, ct);
            if (borrow.IsFailed || borrow.Value == null)
            {
                throw new BorrowNotFoundException($"Udlån med ID {id} blev ikke fundet.");
            }
            else
                return Result.Ok(borrow.Value);
        }
        catch (BorrowNotFoundException ex)
        {
            return Result.Fail("Der er sket en fejl, kunde ikke finde udlån. " + ex.Message);
        }
    }

    public async Task<Result<Borrow>> CreateBorrowAsync(CreateBorrowCommand command, CancellationToken ct = default)
    {
        await _Lock.WaitAsync();
        try
        {
            var existingBorrowsValue = await _borrowRepository.GetBorrowsByUserIdAsync(command.borrowedByUserId, ct);
            if (existingBorrowsValue.IsFailed)
            {
                throw new BorrowNotFoundException("Kunne ikke hente eksisterende udlån.");
            }

            var existingBorrows = existingBorrowsValue.Value ?? Enumerable.Empty<Borrow>();

            try
            {
                var borrow = Borrow.Create(
                    borrowerId: command.borrowedByUserId,
                    lenderId: command.borrowedFromUserId,
                    borrowTimeSlot: command.borrowTimeSlot,
                    existingBorrows: existingBorrows,
                    status: command.status
                );
                await _borrowRepository.CreateBorrowAsync(borrow, ct);
                return Result.Ok(borrow);
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
        catch (BorrowNotFoundException ex)
        {
            return Result.Fail("Der er sket en fejl, kunde ikke finde udlån. " + ex.Message);
        }
        finally
        {
            _Lock.Release();
        }
    }

    public async Task<Result<Borrow>> UpdateBorrowTimeSlotAsync(UpdateBorrowTimeSlotCommand command, CancellationToken ct = default)
    {

        await _Lock.WaitAsync();

        try
        {
            var existingBorrow = await _borrowRepository.GetBorrowByIdAsync(command.id, ct);
            if (existingBorrow.Value == null || existingBorrow.IsFailed)
            {
                throw new BorrowNotFoundException($"Udlån med ID {command.id} blev ikke fundet.");
            }
            var borrow = existingBorrow.Value;

            var existingBorrowsValue = await _borrowRepository.GetBorrowsByUserIdAsync(borrow.BorrowedByUserId, ct);
            if (existingBorrowsValue.IsFailed)
            {
                throw new BorrowNotFoundException("Kunne ikke hente eksisterende udlån.");
            }

            var existingBorrows = existingBorrowsValue.Value ?? Enumerable.Empty<Borrow>();

            try
            {

                borrow.UpdateBorrowTimeSlot(
                    command.borrowTimeSlot,
                    existingBorrows
                );
                await _borrowRepository.UpdateBorrowAsync(borrow, ct);
                return Result.Ok(borrow);
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
        catch (BorrowNotFoundException ex)
        {
            return Result.Fail("Der er sket en fejl, kunde ikke finde udlån. " + ex.Message);
        }
        finally
        {
            _Lock.Release();
        }
    }

    public async Task<Result<Borrow>> UpdateBorrowStatusAsync(UpdateBorrowStatusCommand command, CancellationToken ct = default)
    {

        await _Lock.WaitAsync();

        try
        {
            var existingBorrow = await _borrowRepository.GetBorrowByIdAsync(command.borrowId, ct);
            if (existingBorrow.Value == null || existingBorrow.IsFailed)
            {
                throw new BorrowNotFoundException($"Udlån med ID {command.borrowId} blev ikke fundet.");
            }
            var borrow = existingBorrow.Value;

            try
            {
                if (command.targetStatus == BorrowStatus.Completed)
                {
                    borrow.CompleteBorrow();
                }
                else if (command.targetStatus == BorrowStatus.Cancelled)
                {
                    borrow.CancelBorrow();
                }
                else{}

                await _borrowRepository.UpdateBorrowStatusAsync(borrow, ct);
                return Result.Ok(borrow);
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
        catch (BorrowNotFoundException ex)
        {
            return Result.Fail("Der er sket en fejl, kunde ikke finde udlån. " + ex.Message);
        }
        finally
        {
            _Lock.Release();
        }
    }

    public async Task<Result> DeleteBorrowAsync(DeleteBorrowCommand command, CancellationToken ct = default)
    {
        var existingBorrow = await _borrowRepository.GetBorrowByIdAsync(command.id, ct);
        if (existingBorrow.Value == null || existingBorrow.IsFailed)
        {
            return Result.Fail("Udlån blev ikke fundet.");
        }
        await _borrowRepository.DeleteBorrowAsync(existingBorrow.Value, ct);
        return Result.Ok();
    }

}
