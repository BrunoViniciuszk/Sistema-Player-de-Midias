using Auth.Domain.Repositories;

namespace Auth.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AuthDbContext _context;
        public IAppUserRepository AppUsers { get; }

        public UnitOfWork(AuthDbContext context, IAppUserRepository appUserRepository)
        {
            _context = context;
            AppUsers = appUserRepository;
        }

        public async Task<int> CommitAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
