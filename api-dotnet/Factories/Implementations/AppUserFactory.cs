using api_dotnet.Factories.Interfaces;
using api_dotnet.Models;
using Microsoft.AspNetCore.Identity;

namespace api_dotnet.Factories.Implementations
{
    public class AppUserFactory : IAppUserFactory
    {
        private readonly IPasswordHasher<AppUser> _passwordHasher;

        public AppUserFactory(IPasswordHasher<AppUser> passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public AppUser Create(string username, string password)
        {
            var user = new AppUser
            {
                Username = username
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, password);

            return user;
        }
    }
}
