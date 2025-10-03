using Midia.Domain.Repositories;
using Midia.Infrastructure.Repositories;

namespace Midia.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MediaDbContext _context;
        public IMediaRepository Medias { get; }

        public UnitOfWork(MediaDbContext context)
        {
            _context = context;
            Medias = new MediaRepository(_context);
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
