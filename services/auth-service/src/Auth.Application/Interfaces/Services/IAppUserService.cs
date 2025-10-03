using Auth.Domain.Entities;

namespace Auth.Application.Interfaces.Services
{
    public interface IAppUserService
    {
        Task<AppUser?> AuthenticateAsync(string username, string password);
        Task<AppUser?> GetByUsernameAsync(string username);
        Task<AppUser?> GetByIdAsync(Guid id);
        Task<AppUser> CreateAsync(string username, string password);
        Task UpdatePasswordAsync(Guid userId, string newPassword);
        Task DeleteAsync(Guid userId);
    }
}
