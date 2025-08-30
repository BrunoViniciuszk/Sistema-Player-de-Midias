using api_dotnet.Models;

namespace api_dotnet.Repositories.Interfaces
{
    public interface IMidiaRepository
    {
        Task<IEnumerable<Midia>> GetAllAsync();
        Task<Midia> GetByIdAsync(int id);
        Task<Midia> CreateAsync(Midia midia);
        Task UpdateAsync(Midia midia);
        Task DeleteAsync(Midia id);
        Task RemoveFromPlaylistsAsync(int midiaId);
    }
}
