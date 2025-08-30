using api_dotnet.Data;
using api_dotnet.Models;
using api_dotnet.Models.Dtos;
using api_dotnet.Services.Playlists;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_dotnet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaylistsController : ControllerBase
    {
        private readonly IPlaylistService _playlistService;

        public PlaylistsController(IPlaylistService playlistService)
        {
            _playlistService = playlistService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlaylistDto>>> Get()
        {
            var playlists = await _playlistService.GetAllAsync();
            return Ok(playlists);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<PlaylistDto>> GetById(int id)
        {
            try
            {
                var playlist = await _playlistService.GetByIdAsync(id);
                return Ok(playlist);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<PlaylistNomeDto>> Update(int id, [FromBody] PlaylistNomeDto dto)
        {
            try
            {
                var updated = await _playlistService.UpdateAsync(id, dto.Nome);
                return Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<PlaylistDto>> Create([FromBody] Playlist playlist)
        {
            var created = await _playlistService.CreateAsync(playlist);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _playlistService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpPost("{playlistId}/midias/{midiaId}")]
        public async Task<IActionResult> AddMidia(int playlistId, int midiaId, [FromQuery] bool exibirNoPlayer = true)
        {
            try
            {
                await _playlistService.AddMidiaAsync(playlistId, midiaId, exibirNoPlayer);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException)
            {
                return Conflict("Mídia já associada à playlist");
            }
        }

        [Authorize]
        [HttpPatch("{playlistId}/midias/{midiaId}")]
        public async Task<IActionResult> UpdateExibirNoPlayer(int playlistId, int midiaId, [FromQuery] bool exibirNoPlayer)
        {
            try
            {
                await _playlistService.UpdateExibirNoPlayer(playlistId, midiaId, exibirNoPlayer);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpDelete("{playlistId}/midias/{midiaId}")]
        public async Task<IActionResult> RemoveMidia(int playlistId, int midiaId)
        {
            try
            {
                await _playlistService.RemoveMidiaAsync(playlistId, midiaId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }

}
