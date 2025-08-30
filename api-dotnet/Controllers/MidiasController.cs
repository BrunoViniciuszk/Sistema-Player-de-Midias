using api_dotnet.Data;
using api_dotnet.Models;
using api_dotnet.Models.Dtos;
using api_dotnet.Services.Midias;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace api_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MidiasController : ControllerBase
    {
        private readonly IMidiaService _midiaService;


        public MidiasController(IMidiaService midiaService)
        {
            _midiaService = midiaService;
        }


        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MidiaDto>>> Get()
        {
            var midias = await _midiaService.GetAllAsync();
            return Ok(midias);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<MidiaDto>> GetMidia(int id)
        {
            try
            {
                var midia = await _midiaService.GetByIdAsync(id);
                return Ok(midia);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] string? nome, [FromForm] string? descricao, [FromForm] IFormFile? file)
        {
            try
            {
                var updated = await _midiaService.UpdateAsync(id, nome, descricao, file);
                return Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromForm] string nome, [FromForm] string descricao)
        {
            try
            {
                var midia = await _midiaService.UploadAndCreateMidiaAsync(file, nome, descricao);
                return CreatedAtAction(nameof(GetMidia), new { id = midia.Id }, midia);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
