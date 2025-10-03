using Playlist.Domain.Entities;

namespace Playlist.Domain.Repositories
{
    public interface IPlaylistRepository
    {
        Task<IEnumerable<PlaylistEntity>> GetAllAsync(CancellationToken ct = default);
        Task<PlaylistEntity?> GetByIdAsync(int id, CancellationToken ct = default);
        Task CreateAsync(PlaylistEntity playlist, CancellationToken ct = default);
        Task DeleteAsync(PlaylistEntity playlist, CancellationToken ct = default);
        Task UpdateAsync(PlaylistEntity playlist, CancellationToken ct = default);

        Task<MidiaPlaylist?> GetMidiaPlaylistAsync(int playlistId, int midiaId, CancellationToken ct = default);
        Task AddMidiaPlaylistAsync(MidiaPlaylist midiaPlaylist, CancellationToken ct = default);
        Task RemoveMidiaPlaylistAsync(MidiaPlaylist midiaPlaylist, CancellationToken ct = default);
    }
}
