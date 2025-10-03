using Auth.Domain.Entities;
using Auth.Domain.Repositories;
using Auth.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Repositories
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly AuthDbContext _context;

        public AppUserRepository(AuthDbContext context) => _context = context;

        public async Task<AppUser?> GetByIdAsync(Guid id) =>
            await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

        public async Task<AppUser?> GetByUsernameAsync(string username) =>
            await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);

        public async Task<IEnumerable<AppUser>> GetAllAsync() =>
            await _context.Users.AsNoTracking().ToListAsync();

        public async Task AddAsync(AppUser user) =>
            await _context.Users.AddAsync(user);

        public void Update(AppUser user) =>
            _context.Users.Update(user);

        public void Remove(AppUser user) =>
            _context.Users.Remove(user);
    }
}
