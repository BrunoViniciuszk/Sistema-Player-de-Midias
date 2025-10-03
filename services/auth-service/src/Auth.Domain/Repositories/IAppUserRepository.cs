using Auth.Domain.Entities;

namespace Auth.Domain.Repositories
{
    public interface IAppUserRepository
    {
        Task<AppUser?> GetByIdAsync(Guid id);
        Task<AppUser?> GetByUsernameAsync(string username);
        Task<IEnumerable<AppUser>> GetAllAsync();

        Task AddAsync(AppUser user);
        void Update(AppUser user);
        void Remove(AppUser user);
    }
}
