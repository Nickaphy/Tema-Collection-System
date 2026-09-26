using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WatchWorld.Api.Requests.ImageRequests;
using WatchWorld.Application.Commands.ImageCommands;
using WatchWorld.Application.Ports.InBound;
using WatchWorld.Domain.Entities;

namespace WatchWorld.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IImagesUseCase _imageUseCase;

        public ImageController(IImagesUseCase imageUseCase)
        {
            _imageUseCase = imageUseCase;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<HighResImage>>> Get(CancellationToken ct)
        {
            var images = await _imageUseCase.GetAllAsync(ct);
            if (images.IsFailed)
                return Problem(string.Join("; ", images.Errors.Select(e => e.Message)));
            return Ok(images.Value);
        }

        [HttpPost]
        //[Authorize(Roles = "User,Admin")] // Commented out because Auth hasn't been enabled yet
        public async Task<ActionResult<HighResImage>> Create([FromBody] CreateImageRequest request, CancellationToken ct)
        {
            var command = new CreateImageCommand(
                url: request.url,
                height: request.height,
                width: request.width
            );
            var result = await _imageUseCase.CreateImageAsync(command, ct);
            if (result.IsFailed)
                return Problem(string.Join("; ", result.Errors.Select(e => e.Message)));

            return Created($"/api/Image/{result.Value.Id}", result.Value);
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "User,Admin")] // Commented out because Auth hasn't been enabled yet
        public async Task<ActionResult> Delete(Guid id, CancellationToken ct)
        {
            await _imageUseCase.DeleteImageAsync(id, ct);
            return NoContent();
        }
    }
}
