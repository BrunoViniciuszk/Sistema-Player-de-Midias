using api_dotnet.Models;

namespace api_dotnet.Repositories.Interfaces
{
    public interface IPlaylistRepository
    {
        Task<IEnumerable<Playlist>> GetAllAsync();
        Task<Playlist> GetByIdAsync(int id);
        Task<Playlist> CreateAsync(Playlist playlist);
        Task DeleteAsync(Playlist playlist);
        Task<MidiaPlaylist> GetMidiaPlaylistAsync(int playlistId, int midiaId);
        Task AddMidiaPlaylistAsync(MidiaPlaylist midiaPlaylist);
        Task RemoveMidiaPlaylistAsync(MidiaPlaylist midiaPlaylist);
        Task SaveChangesAsync();
    }
}
