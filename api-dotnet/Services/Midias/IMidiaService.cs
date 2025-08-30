using api_dotnet.Models;

namespace api_dotnet.Services.Midias
{
    public interface IMidiaService
    {
        Task<IEnumerable<Midia>> GetAllAsync();
        Task<Midia> GetByIdAsync(int id);
        Task<Midia> UploadAndCreateMidiaAsync(IFormFile file, string nome, string descricao);
        Task<Midia> UpdateAsync(int id, string? nome, string? descricao, IFormFile? file);
        Task DeleteAsync(int id);
    }
}
