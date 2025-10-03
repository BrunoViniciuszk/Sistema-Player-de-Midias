using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Playlist.Application.Dtos;
using Playlist.Application.Interfaces;
using Playlist.Api.Models.Requests;
using System.Net.Mime;

namespace Playlist.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class PlaylistsController : ControllerBase
    {
        private readonly IPlaylistService _service;

        public PlaylistsController(IPlaylistService service) => _service = service;

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PlaylistDto>), 200)]
        public async Task<ActionResult<IEnumerable<PlaylistDto>>> Get(CancellationToken ct)
        {
            var result = await _service.GetAllAsync(ct);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(PlaylistDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PlaylistDto>> GetById(int id, CancellationToken ct)
        {
            var result = await _service.GetByIdAsync(id, ct);
            return result is null ? NotFound() : Ok(result);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(PlaylistDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<PlaylistDto>> Create([FromBody] CreatePlaylistRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _service.CreateAsync(request.Nome, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [Authorize]
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(PlaylistDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<PlaylistDto>> Update(int id, [FromBody] UpdatePlaylistRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _service.UpdateAsync(id, request.Nome, ct);
            return updated is null ? NotFound() : Ok(updated);
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var deleted = await _service.DeleteAsync(id, ct);
            return deleted ? NoContent() : NotFound();
        }

        [Authorize]
        [HttpPost("{playlistId:int}/midias/{midiaId:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(409)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AddMidia(int playlistId, int midiaId, [FromQuery] bool exibirNoPlayer = true, CancellationToken ct = default)
        {
            var ok = await _service.AddMidiaAsync(playlistId, midiaId, exibirNoPlayer, ct);
            if (ok) return NoContent();

            return Conflict("Mídia já associada ou playlist/mídia inexistente");
        }

        [Authorize]
        [HttpPatch("{playlistId:int}/midias/{midiaId:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateExibirNoPlayer(int playlistId, int midiaId, [FromQuery] bool exibirNoPlayer, CancellationToken ct)
        {
            var ok = await _service.UpdateExibirNoPlayerAsync(playlistId, midiaId, exibirNoPlayer, ct);
            return ok ? NoContent() : NotFound();
        }

        [Authorize]
        [HttpDelete("{playlistId:int}/midias/{midiaId:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RemoveMidia(int playlistId, int midiaId, CancellationToken ct)
        {
            var ok = await _service.RemoveMidiaAsync(playlistId, midiaId, ct);
            return ok ? NoContent() : NotFound();
        }
    }
}
