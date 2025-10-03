using Auth.Application.Dtos;
using Auth.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
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
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest(new { message = "Username e senha são obrigatórios" });

            var user = await _userService.AuthenticateAsync(dto.Username, dto.Password);

            if (user == null)
                return Unauthorized(new { message = "Usuário ou senha inválidos" });

            var token = _authService.GenerateJwtToken(user);

            return Ok(new LoginResponseDto
            {
                Token = token,
                Username = user.Username
            });
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest(new { message = "Username e senha são obrigatórios" });

            var existingUser = await _userService.GetByUsernameAsync(dto.Username);
            if (existingUser != null)
                return BadRequest(new { message = "Usuário já existe" });

            var user = await _userService.CreateAsync(dto.Username, dto.Password);

            return Ok(new
            {
                message = "Usuário criado com sucesso",
                userId = user.Id,
                username = user.Username
            });
        }
    }
}
