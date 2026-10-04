using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WatchWorld.Api.Requests.BrandRequests;
using WatchWorld.Application.Commands.BrandCommands;
using WatchWorld.Application.Ports.InBound;
using WatchWorld.Domain.Entities;

namespace WatchWorld.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandController : ControllerBase
    {
        private readonly IBrandUseCase _brandUseCase;

        public BrandController(IBrandUseCase brandUseCase)
        {
            _brandUseCase = brandUseCase;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Brand>>> GetAllBrandsAsync(CancellationToken ct)
        {
            var result = await _brandUseCase.GetAllBrandsAsync(ct);

            if (result.IsFailed)
                return Problem(string.Join("; ", result.Errors.Select(e => e.Message)));

            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Brand>> GetBrandsByIdAsync(Guid id, CancellationToken ct)
        {
            var brand = await _brandUseCase.GetBrandByIdAsync(id, ct);
            return Ok(brand);
        }

        [HttpPost]
        //[Authorize(Roles = "User,Admin")] // Commented out because Auth hasn't been enabled yet
        public async Task<ActionResult<Brand>> CreateBrandAsync([FromBody] CreateBrandRequest request, CancellationToken ct)
        {
            var command = new CreateBrandCommand(
                name: request.name,
                foundingYear: request.foundingYear,
                countryOfOrigin: request.countryOfOrigin,
                parentGroup: request.parentGroup,
                logoUrl: request.logoUrl,
                websiteUrl: request.websiteUrl,
                founderName: request.founderName,
                founderDescriptionNote: request.founderDescriptionNote,
                originDescriptionNote: request.originDescriptionNote,
                brandDescriptionNote: request.brandDescriptionNote,
                watchStyleDescriptionNote: request.watchStyleDescriptionNote,
                isActive: request.isActive
            );
            var result = await _brandUseCase.CreateBrandAsync(command, ct);
            if (result.IsFailed)
                return Problem(string.Join("; ", result.Errors.Select(e => e.Message)));

            return CreatedAtAction(nameof(GetBrandsByIdAsync), new { id = result.Value.Id }, result.Value);

        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "User,Admin")] // Commented out because Auth hasn't been enabled yet
        public async Task<ActionResult> Delete(Guid id, CancellationToken ct)
        {
            await _brandUseCase.DeleteBrandAsync(id, ct);
            return NoContent();
        }
    }
}
