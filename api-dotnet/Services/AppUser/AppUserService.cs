using api_dotnet.Data;
using api_dotnet.Factories.Interfaces;
using api_dotnet.Models;
using api_dotnet.Services.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api_dotnet.Services.Auth
{
    public class AppUserService : IAppUserService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<AppUser> _passwordHasher;
        private readonly IAppUserFactory _userFactory;

        public AppUserService(
            AppDbContext context,
            IPasswordHasher<AppUser> passwordHasher,
            IAppUserFactory userFactory)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _userFactory = userFactory;
        }

        public async Task<AppUser> Authenticate(string username, string password)
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.Username == username);

            if (user == null) return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success ? user : null;
        }

        public async Task<AppUser> GetByUsername(string username)
        {
            return await _context.Users
                .SingleOrDefaultAsync(u => u.Username == username);
        }

        public async Task Create(string username, string password)
        {
            var user = _userFactory.Create(username, password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
