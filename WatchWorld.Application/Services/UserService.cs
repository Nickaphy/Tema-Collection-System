using System.Diagnostics;
using FluentResults;
using WatchWorld.Application.Commands.UserCommands;
using WatchWorld.Application.Ports.InBound;
using WatchWorld.Application.Ports.OutBound;
using WatchWorld.Application.Ports.OutBound.Services;
using WatchWorld.Application.Results;
using WatchWorld.Domain.Entities;
using WatchWorld.Domain.Service;

namespace WatchWorld.Application.Services;

public class UserService : IUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _tokenGenerator;
    private static readonly SemaphoreSlim _Lock = new(1, 1);

    public UserService(IUserRepository userRepository, IJwtTokenGenerator tokenGenerator)
    {
        _userRepository = userRepository;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<Result<IEnumerable<User>>> GetAllUsersAsync(CancellationToken ct = default)
    {
        var user = await _userRepository.GetAllUsersAsync(ct);
        return Result.Ok(user.Value);
    }

    public async Task<Result<User>> GetUserByIdAsync(Guid id, CancellationToken ct = default)
    {
        try
        {
            var user = await _userRepository.GetUserByIdAsync(id, ct);
            if (user.IsFailed || user.Value == null)
            {
                throw new UserNotFoundException($"Brugeren med ID {id} blev ikke fundet.");
            }
            else
                return Result.Ok(user.Value);
        }
        catch (UserNotFoundException ex)
        {
            return Result.Fail("Der er sket en fejl, kunde ikke finde brugeren. " + ex.Message);
        }
    }

    public async Task<Result<LogInResult>> LogInAsync(LogInCommand command, CancellationToken ct = default)
    {
        try
        {
            if (command.password.IsWhiteSpace() || command.password == null)
                return Result.Fail("Forkert adgangskode");

            var name = $"{command.firstName} {command.lastName}";

            var user = await _userRepository.GetUserByLoginCredentialsAsync(command.email, name, command.password, ct);
            if (user.IsFailed)
            {
                return Result.Fail("Forkert brugernavn eller password. Kontakt support, hvis du har brug for hjælp.");
            }

            // Hvis brugeren er ægte og får logget ind, printer vi dem her deres token/keycard.
            var loggedInUser = user.Value;
            var token = _tokenGenerator.GenerateToken(loggedInUser.Id, loggedInUser.Email, loggedInUser.IsAdmin);

            return Result.Ok(new LogInResult(
                id: loggedInUser.Id,
                firstName: loggedInUser.FirstName,
                lastName: loggedInUser.LastName,
                phoneNumber: loggedInUser.PhoneNumber,
                email: loggedInUser.Email,
                address: loggedInUser.Address,
                city: loggedInUser.City,
                note: loggedInUser.Note,
                isAdmin: loggedInUser.IsAdmin,
                token: token
            ));
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
            Debug.WriteLine($"Infrastructure Failure: {ex.Message}");
            return Result.Fail("An unexpected system error occurred." + ex.Message);
        }
    }

    public async Task<Result<User>> CreateUserAsync(CreateUserCommand command, CancellationToken ct = default)
    {
        var existingUser = await _userRepository.GetAllUsersAsync(ct);

        await _Lock.WaitAsync();
        try
        {
            try
            {
                var user = User.Create(
                    firstName: command.firstName,
                    lastName: command.lastName,
                    phoneNumber: command.phoneNumber,
                    email: command.email,
                    address: command.address,
                    city: command.city,
                    note: command.note,
                    password: command.password,
                    rating: command.rating
            );
                await _userRepository.CreateUserAsync(user, ct);
                return Result.Ok(user);
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
                Debug.WriteLine($"Infrastructure Failure: {ex.Message}");
                return Result.Fail("An unexpected system error occurred." + ex.Message);
            }
        }
        finally
        {
            _Lock.Release();
        }
    }

    public async Task<Result<User>> UpdateUserAsync(UpdateUserCommand command, CancellationToken ct = default)
    {

        await _Lock.WaitAsync();

        try
        {
            var existingUser = await _userRepository.GetUserByIdAsync(command.id, ct);
            if (existingUser.Value == null || existingUser.IsFailed)
            {
                throw new UserNotFoundException($"Brugeren med ID {command.id} blev ikke fundet.");
            }
            var user = existingUser.Value;
            try
            {

                user.UpdateUser(
                    command.firstName,
                    command.lastName,
                    command.phoneNumber,
                    command.email,
                    command.address,
                    command.city,
                    command.note,
                    command.password
                    );
                await _userRepository.UpdateUserAsync(user, ct);
                return Result.Ok(user);
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
                Debug.WriteLine($"Infrastructure Failure: {ex.Message}");
                return Result.Fail("An unexpected system error occurred." + ex.Message);
            }
        }
        catch (UserNotFoundException ex)
        {
            return Result.Fail("Der er sket en fejl, kunde ikke finde brugeren. " + ex.Message);
        }
        finally
        {
            _Lock.Release();
        }
    }

    public async Task<Result> DeleteUserAsync(DeleteUserCommand command, CancellationToken ct = default)
    {
        var existingUser = await _userRepository.GetUserByIdAsync(command.userId, ct);
        if (existingUser.Value == null || existingUser.IsFailed)
        {
            return Result.Fail("Brugeren blev ikke fundet.");
        }
        await _userRepository.DeleteUserAsync(command.userId, ct);
        return Result.Ok();
    }

    public async Task<Result> SetAdminRoleAsync(SetAdminRoleCommand command, CancellationToken ct = default)
    {
        var userResult = await _userRepository.GetUserByIdAsync(command.userId, ct);
        if (userResult.IsFailed || userResult.Value == null)
        {
            return Result.Fail("Brugeren kunne ikke findes");
        }
        try
        {
            var user = userResult.Value;
            user.SetAdmin(user);
            bool shouldBeAdmin = user.IsAdmin;
            await _userRepository.SetUserAsAdminAsync(command.userId, shouldBeAdmin, ct);
            return Result.Ok();
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
            Debug.WriteLine($"Infrastructure Failure: {ex.Message}");
            return Result.Fail("An unexpected system error occurred." + ex.Message);
        }

    }
}