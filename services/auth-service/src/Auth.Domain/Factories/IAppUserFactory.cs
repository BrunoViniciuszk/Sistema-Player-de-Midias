using Auth.Domain.Entities;

namespace Auth.Domain.Factories
{
    public interface IAppUserFactory
    {
        AppUser Create(string username, string password);
    }
}
