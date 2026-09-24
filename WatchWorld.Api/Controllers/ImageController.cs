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
            return Ok(images);
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<HighResImage>> Create([FromBody] CreateImageRequest request, CancellationToken ct)
        {
            var command = new CreateImageCommand(
                url: request.url,
                height: request.height,
                width: request.width
            );

            var image = await _imageUseCase.CreateImageAsync(command, ct);
            return CreatedAtAction(nameof(Get), new { id = new Guid()}, image);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult> Delete(Guid id, CancellationToken ct)
        {
            await _imageUseCase.DeleteImageAsync(id, ct);
            return NoContent();
        }
    }
}
