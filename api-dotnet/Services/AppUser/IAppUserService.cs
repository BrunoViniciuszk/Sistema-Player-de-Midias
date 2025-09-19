using api_dotnet.Models;

namespace api_dotnet.Services.User
{
    public interface IAppUserService
    {
        Task<AppUser> Authenticate(string username, string password);
        Task<AppUser> GetByUsername(string username);
        Task Create(string username, string password);
    }
}
