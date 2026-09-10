using FluentResults;
using WatchWorld.Application.Commands.ImageCommands;
using WatchWorld.Application.Ports.InBound;
using WatchWorld.Application.Ports.OutBound;
using WatchWorld.Domain.Entities;
using WatchWorld.Domain.Service;

namespace WatchWorld.Application.Services;

public class ImageService : IImagesUseCase
{
    private readonly IImageRepository _repository;
    private static readonly SemaphoreSlim _Lock = new(1, 1);

    public ImageService(IImageRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<HighResImage?>>> GetAllAsync(CancellationToken ct = default)
    {
        var images = await _repository.GetAllAsync(ct);
        return Result.Ok(images.Value);
    }

    public async Task<Result<HighResImage>> GetImageByIdAsync(Guid id, CancellationToken ct = default)
    {
        var image = await _repository.GetImageByIdAsync(id.GetHashCode(), ct);
        if (image.IsFailed || image.Value == null)
        {
            return Result.Fail("Billedet blev ikke fundet.");
        }
        return Result.Ok(image.Value);
    }

    public async Task<Result<HighResImage>> CreateImageAsync(CreateImageCommand command, CancellationToken ct = default)
    {
        var existingImages = await _repository.GetAllAsync(ct);

        await _Lock.WaitAsync();
        try
        {
            try
            {

                var image = HighResImage.Create(
                    url: command.url,
                    height: command.height,
                    width: command.width
                );
                await _repository.CreateImageAsync(image);
                return Result.Ok(image);
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

    public async Task<Result> DeleteImageAsync(Guid id, CancellationToken ct = default)
    {
        var existingImage = await _repository.GetImageByIdAsync(id.GetHashCode(), ct);
        if (existingImage == null)
        {
            return Result.Fail("Billedet blev ikke fundet.");
        }
        await _repository.DeleteImageAsync(id, ct);
        return Result.Ok();
    }
}
