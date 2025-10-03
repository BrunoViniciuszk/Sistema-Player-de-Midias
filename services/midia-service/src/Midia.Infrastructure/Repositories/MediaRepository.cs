using Microsoft.EntityFrameworkCore;
using Midia.Domain.Entities;
using Midia.Domain.Repositories;
using Midia.Infrastructure.Data;

namespace Midia.Infrastructure.Repositories
{
    public class MediaRepository : IMediaRepository
    {
        private readonly MediaDbContext _context;

        public MediaRepository(MediaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Media>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Midias
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<Media?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Midias
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }

        public async Task<Media> CreateAsync(Media midia, CancellationToken cancellationToken = default)
        {
            await _context.Midias.AddAsync(midia, cancellationToken);
            return midia;
        }

        public Task UpdateAsync(Media midia, CancellationToken cancellationToken = default)
        {
            _context.Midias.Update(midia);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Media midia, CancellationToken cancellationToken = default)
        {
            _context.Midias.Remove(midia);
            return Task.CompletedTask;
        }
    }
}
