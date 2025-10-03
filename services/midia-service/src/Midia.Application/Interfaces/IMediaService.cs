using Midia.Application.Dtos;

namespace Midia.Application.Interfaces
{
    public interface IMediaService
    {
        Task<IEnumerable<MediaDto>> GetAllAsync();
        Task<MediaDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<MediaDto> UploadAndCreateMidiaAsync(UploadMediaDto dto, CancellationToken cancellationToken = default);
        Task<MediaDto> UpdateAsync(int id, UpdateMediaDto dto, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
