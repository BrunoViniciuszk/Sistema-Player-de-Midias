using Auth.Domain.Entities;

namespace Auth.Application.Interfaces.Services
{
    public interface IAuthService
    {
        string GenerateJwtToken(AppUser user);
    }
}
