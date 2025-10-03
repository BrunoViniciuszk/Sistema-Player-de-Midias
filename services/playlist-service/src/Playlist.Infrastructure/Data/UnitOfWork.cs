using Playlist.Domain.Repositories;

namespace Playlist.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PlaylistDbContext _db;
        public IPlaylistRepository Playlists { get; }

        public UnitOfWork(PlaylistDbContext db, IPlaylistRepository playlists)
        {
            _db = db;
            Playlists = playlists;
        }

        public async Task<int> CommitAsync(CancellationToken ct = default)
            => await _db.SaveChangesAsync(ct);

        public void Dispose() => _db.Dispose();
    }
}
