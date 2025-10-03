using Midia.Domain.Entities;

namespace Midia.Domain.Repositories
{
    public interface IMediaRepository
    {
        Task<IEnumerable<Media>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Media?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Media> CreateAsync(Media midia, CancellationToken cancellationToken = default);
        Task UpdateAsync(Media midia, CancellationToken cancellationToken = default);
        Task DeleteAsync(Media midia, CancellationToken cancellationToken = default);
    }
}
