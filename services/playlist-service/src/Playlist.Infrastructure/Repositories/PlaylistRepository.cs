using Microsoft.EntityFrameworkCore;
using Playlist.Domain.Entities;
using Playlist.Domain.Repositories;
using Playlist.Infrastructure.Data;

namespace Playlist.Infrastructure.Repositories
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly PlaylistDbContext _db;
        public PlaylistRepository(PlaylistDbContext db) => _db = db;

        public async Task<IEnumerable<PlaylistEntity>> GetAllAsync(CancellationToken ct = default)
        {
            return await _db.Playlists
                .Include(p => p.Midias) 
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<PlaylistEntity?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _db.Playlists
                .Include(p => p.Midias)
                .FirstOrDefaultAsync(p => p.Id == id, ct);
        }

        public async Task CreateAsync(PlaylistEntity playlist, CancellationToken ct = default)
        {
            await _db.Playlists.AddAsync(playlist, ct);
        }

        public Task DeleteAsync(PlaylistEntity playlist, CancellationToken ct = default)
        {
            _db.Playlists.Remove(playlist);
            return Task.CompletedTask;
        }

        public async Task UpdateAsync(PlaylistEntity playlist, CancellationToken ct = default)
        {
            _db.Playlists.Update(playlist);
            await Task.CompletedTask; 
        }

        public async Task<MidiaPlaylist?> GetMidiaPlaylistAsync(int playlistId, int midiaId, CancellationToken ct = default)
        {
            return await _db.MidiaPlaylists
                .FirstOrDefaultAsync(mp => mp.PlaylistId == playlistId && mp.MidiaId == midiaId, ct);
        }

        public async Task AddMidiaPlaylistAsync(MidiaPlaylist midiaPlaylist, CancellationToken ct = default)
        {
            await _db.MidiaPlaylists.AddAsync(midiaPlaylist, ct);
        }

        public Task RemoveMidiaPlaylistAsync(MidiaPlaylist midiaPlaylist, CancellationToken ct = default)
        {
            _db.MidiaPlaylists.Remove(midiaPlaylist);
            return Task.CompletedTask;
        }
    }
}
