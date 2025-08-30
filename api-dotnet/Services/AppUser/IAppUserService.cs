using api_dotnet.Models;

namespace api_dotnet.Services.User
{
    public interface IAppUserService
    {
        AppUser Authenticate(string username, string password);
        AppUser GetByUsername(string username);
        void Create(AppUser user, string password);
    }
}
