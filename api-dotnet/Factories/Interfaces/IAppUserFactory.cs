using api_dotnet.Models;

namespace api_dotnet.Factories.Interfaces
{
    public interface IAppUserFactory
    {
        AppUser Create(string username, string password);
    }
}
