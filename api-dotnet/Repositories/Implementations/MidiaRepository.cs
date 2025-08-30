using api_dotnet.Models;
using api_dotnet.Data;
using Microsoft.EntityFrameworkCore;
using api_dotnet.Repositories.Interfaces;

namespace api_dotnet.Repositories.Implementations
{
    public class MidiaRepository : IMidiaRepository
    {
        private readonly AppDbContext _context;

        public MidiaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Midia>> GetAllAsync()
        {
            return await _context.Midias.AsNoTracking().ToListAsync();
        }

        public async Task<Midia> GetByIdAsync(int id)
        {
            return await _context.Midias.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
        }


        public async Task<Midia> CreateAsync(Midia midia)
        {
            await _context.Midias.AddAsync(midia);
            await _context.SaveChangesAsync();
            return midia;
        }

        public async Task UpdateAsync(Midia midia)
        {
            _context.Midias.Update(midia);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Midia midia)
        {
            _context.Midias.Remove(midia);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFromPlaylistsAsync(int midiaId)
        {
            var relacoes = _context.MidiaPlaylists.Where(mp => mp.MidiaId == midiaId);
            _context.MidiaPlaylists.RemoveRange(relacoes);
            await _context.SaveChangesAsync();
        }

    }
}
