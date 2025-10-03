using Playlist.Application.Dtos;

namespace Playlist.Application.Interfaces
{
    public interface IPlaylistService
    {
        Task<IEnumerable<PlaylistDto>> GetAllAsync(CancellationToken ct = default);
        Task<PlaylistDto?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<PlaylistDto> CreateAsync(string nome, CancellationToken ct = default);
        Task<PlaylistDto?> UpdateAsync(int id, string nome, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);

        Task<bool> AddMidiaAsync(int playlistId, int midiaId, bool exibirNoPlayer = true, CancellationToken ct = default);
        Task<bool> RemoveMidiaAsync(int playlistId, int midiaId, CancellationToken ct = default);
        Task<bool> UpdateExibirNoPlayerAsync(int playlistId, int midiaId, bool exibirNoPlayer, CancellationToken ct = default);
    }
}
