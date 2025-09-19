using api_dotnet.Models;
using api_dotnet.Models.Dtos;

namespace api_dotnet.Services.Midias
{
    public interface IMidiaService
    {
        Task<IEnumerable<Midia>> GetAllAsync();
        Task<Midia> GetByIdAsync(int id);
        Task<Midia> UploadAndCreateMidiaAsync(UploadMidiaDto dto);
        Task<Midia> UpdateAsync(int id, UpdateMidiaDto dto);
        Task DeleteAsync(int id);
    }
}
