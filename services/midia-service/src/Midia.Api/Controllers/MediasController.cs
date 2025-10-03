using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Midia.Application.Dtos;
using Midia.Application.Interfaces;

namespace Midia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediasController : ControllerBase
    {
        private readonly IMediaService _midiaService;

        public MediasController(IMediaService midiaService)
        {
            _midiaService = midiaService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MediaDto>>> Get()
        {
            var dtos = await _midiaService.GetAllAsync();
            return Ok(dtos);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<MediaDto>> GetById(int id)
        {
            try
            {
                var dto = await _midiaService.GetByIdAsync(id);
                return Ok(dto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(object), 201)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        public async Task<IActionResult> Upload([FromForm] UploadMediaDto dto, CancellationToken cancellationToken)
        {
            var created = await _midiaService.UploadAndCreateMidiaAsync(dto, cancellationToken);
            
            return CreatedAtAction(nameof(GetById), new { id = created.Id, version = "1.0" }, created);
        }

        [Authorize]
        [HttpPut("{id:int}")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateMediaDto dto, CancellationToken cancellationToken)
        {
            var updated = await _midiaService.UpdateAsync(id, dto, cancellationToken);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _midiaService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
