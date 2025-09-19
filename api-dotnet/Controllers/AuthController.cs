using api_dotnet.Models.Dtos;
using api_dotnet.Services.Auth;
using api_dotnet.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace api_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAppUserService _userService;
        private readonly IAuthService _authService;

        public AuthController(IAppUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userService.Authenticate(dto.Username, dto.Password);
            if (user == null)
                return Unauthorized(new { message = "Usuário ou senha inválidos" });

            var token = _authService.GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var existingUser = await _userService.GetByUsername(dto.Username);
            if (existingUser != null)
                return BadRequest("Usuário já existe");

            await _userService.Create(dto.Username, dto.Password);

            return Ok("Usuário criado com sucesso");
        }
    }
}
