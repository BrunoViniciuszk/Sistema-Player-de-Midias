using api_dotnet.Models;

namespace api_dotnet.Services.Auth
{
    public interface IAuthService
    {
        string GenerateJwtToken(AppUser user);
    }
}
