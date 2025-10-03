using Auth.Domain.Entities;
using Auth.Domain.Factories;
using Microsoft.AspNetCore.Identity;

namespace Auth.Infrastructure.Factories
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
            var user = new AppUser(username, string.Empty);
            user.SetPasswordHash(_passwordHasher.HashPassword(user, password));
            return user;
        }
    }
}
