using Auth.Application.Interfaces.Services;
using Auth.Domain.Entities;
using Auth.Domain.Factories;
using Auth.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Auth.Application.Services
{
    public class AppUserService : IAppUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<AppUser> _passwordHasher;
        private readonly IAppUserFactory _userFactory;

        public AppUserService(
            IUnitOfWork unitOfWork,
            IPasswordHasher<AppUser> passwordHasher,
            IAppUserFactory userFactory)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _userFactory = userFactory;
        }

        public async Task<AppUser?> AuthenticateAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Username e senha são obrigatórios.");

            var user = await _unitOfWork.AppUsers.GetByUsernameAsync(username);
            if (user == null) return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success ? user : null;
        }

        public async Task<AppUser?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id inválido.");

            return await _unitOfWork.AppUsers.GetByIdAsync(id);
        }

        public async Task<AppUser?> GetByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username é obrigatório.");

            return await _unitOfWork.AppUsers.GetByUsernameAsync(username);
        }

        public async Task<AppUser> CreateAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Username e senha são obrigatórios.");

            var existingUser = await _unitOfWork.AppUsers.GetByUsernameAsync(username);
            if (existingUser != null)
                throw new InvalidOperationException("Usuário já existe.");

            var user = _userFactory.Create(username, password);

            await _unitOfWork.AppUsers.AddAsync(user);
            await _unitOfWork.CommitAsync();

            return user;
        }

        public async Task UpdatePasswordAsync(Guid userId, string newPassword)
        {
            if (userId == Guid.Empty || string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentException("Id e nova senha são obrigatórios.");

            var user = await _unitOfWork.AppUsers.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("Usuário não encontrado.");

            user.SetPasswordHash(_passwordHasher.HashPassword(user, newPassword));

            _unitOfWork.AppUsers.Update(user);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("Id inválido.");

            var user = await _unitOfWork.AppUsers.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("Usuário não encontrado.");

            _unitOfWork.AppUsers.Remove(user);
            await _unitOfWork.CommitAsync();
        }
    }
}
