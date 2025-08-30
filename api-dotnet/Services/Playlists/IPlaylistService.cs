using api_dotnet.Models;
using api_dotnet.Models.Dtos;

namespace api_dotnet.Services.Playlists
{
    public interface IPlaylistService
    {
        Task<IEnumerable<PlaylistDto>> GetAllAsync();
        Task<PlaylistDto> GetByIdAsync(int id);
        Task<PlaylistDto> CreateAsync(Playlist playlist);
        Task<PlaylistNomeDto> UpdateAsync(int id, string novoNome);
        Task DeleteAsync(int id);
        Task AddMidiaAsync(int playlistId, int midiaId, bool exibirNoPlayer = true);
        Task RemoveMidiaAsync(int playlistId, int midiaId);
        Task UpdateExibirNoPlayer(int playlistId, int midiaId, bool exibirNoPlayer);
    }
}
