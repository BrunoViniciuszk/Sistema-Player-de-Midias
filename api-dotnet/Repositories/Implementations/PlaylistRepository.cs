using api_dotnet.Data;
using api_dotnet.Models;
using api_dotnet.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api_dotnet.Repositories.Implementations
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly AppDbContext _context;
        public PlaylistRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Playlist>> GetAllAsync()
        {
            return await _context.Playlists
                .Include(p => p.Midias)
                .ThenInclude(mp => mp.Midia)
                .ToListAsync();
        }

        public async Task<Playlist> GetByIdAsync(int id)
        {
            return await _context.Playlists
                .Include(p => p.Midias)
                .ThenInclude(mp => mp.Midia)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Playlist> CreateAsync(Playlist playlist)
        {
            await _context.Playlists.AddAsync(playlist);
            await _context.SaveChangesAsync();
            return playlist;
        }

        public async Task DeleteAsync(Playlist playlist)
        {
            _context.MidiaPlaylists.RemoveRange(playlist.Midias);
            _context.Playlists.Remove(playlist);
            await _context.SaveChangesAsync();
        }

        public async Task<MidiaPlaylist> GetMidiaPlaylistAsync(int playlistId, int midiaId)
        {
            return await _context.MidiaPlaylists
                .FindAsync(playlistId, midiaId);
        }

        public async Task AddMidiaPlaylistAsync(MidiaPlaylist midiaPlaylist)
        {
            await _context.MidiaPlaylists.AddAsync(midiaPlaylist);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveMidiaPlaylistAsync(MidiaPlaylist midiaPlaylist)
        {
            _context.MidiaPlaylists.Remove(midiaPlaylist);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
