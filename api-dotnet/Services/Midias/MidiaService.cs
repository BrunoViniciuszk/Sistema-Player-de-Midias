using api_dotnet.Data;
using api_dotnet.Factories.Interfaces;
using api_dotnet.Models;
using api_dotnet.Models.Dtos;
using api_dotnet.Repositories.Interfaces;
using api_dotnet.Storage.Strategy.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace api_dotnet.Services.Midias
{
    public class MidiaService : IMidiaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;
        private readonly IMidiaFactory _midiaFactory;
        private readonly IStorageStrategy _storageStrategy;

        private static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png" };
        private static readonly string[] AllowedVideoExtensions = { ".mp4", ".avi", ".mov" };

        public MidiaService(IUnitOfWork unitOfWork, IWebHostEnvironment env, IMidiaFactory midiaFactory, IStorageStrategy storageStrategy)
        {
            _unitOfWork = unitOfWork;
            _env = env;
            _midiaFactory = midiaFactory;
            _storageStrategy = storageStrategy;
        }

        public async Task<IEnumerable<Midia>> GetAllAsync()
        {
            return await _unitOfWork.Midias.GetAllAsync();
        }

        public async Task<Midia> GetByIdAsync(int id)
        {
            var midia = await _unitOfWork.Midias.GetByIdAsync(id);
            if (midia == null) throw new KeyNotFoundException("Mídia não encontrada");
            return midia;
        }

        public async Task<Midia> UploadAndCreateMidiaAsync(UploadMidiaDto dto)
        {
            var fileName = Guid.NewGuid() + Path.GetFileName(dto.File.FileName);
            var url = await _storageStrategy.SaveAsync(dto.File, fileName);

            var midia = _midiaFactory.Create(dto.Nome, dto.Descricao, url);

            var created = await _unitOfWork.Midias.CreateAsync(midia);
            await _unitOfWork.CommitAsync();

            return created;
        }

        public async Task<Midia> UpdateAsync(int id, UpdateMidiaDto dto)
        {
            var existing = await GetByIdAsync(id);

            existing.Nome = dto.Nome ?? existing.Nome;
            existing.Descricao = dto.Descricao ?? existing.Descricao;

            if (dto.File != null)
            {
                DeleteFileIfExists(existing.UrlMidia);
                existing.UrlMidia = await _storageStrategy.SaveAsync(dto.File, Guid.NewGuid() + Path.GetFileName(dto.File.FileName));
            }

            await _unitOfWork.Midias.UpdateAsync(existing);
            await _unitOfWork.CommitAsync();

            return existing;
        }

        public async Task DeleteAsync(int id)
        {
            var midia = await _unitOfWork.Midias.GetByIdAsync(id);

            await _unitOfWork.Midias.RemoveFromPlaylistsAsync(id);

            await _storageStrategy.DeleteAsync(midia.UrlMidia);

            await _unitOfWork.Midias.DeleteAsync(midia);
            await _unitOfWork.CommitAsync();
        }

        #region Helpers

        private void DeleteFileIfExists(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath)) return;

            var filePath = Path.Combine(
                _env.ContentRootPath,
                relativePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString())
            );

            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        private async Task<string> SaveFileAndGetUrl(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Arquivo inválido");

            var ext = Path.GetExtension(file.FileName).ToLower();
            if (!AllowedImageExtensions.Contains(ext) && !AllowedVideoExtensions.Contains(ext))
                throw new ArgumentException("Formato de arquivo não suportado");

            string subFolder = AllowedImageExtensions.Contains(ext) ? "Imagens" : "Videos";

            var uploadDir = Path.Combine(_env.ContentRootPath, "Uploads", subFolder);
            if (!Directory.Exists(uploadDir))
                Directory.CreateDirectory(uploadDir);

            var uniqueFileName = Guid.NewGuid().ToString() + ext;
            var filePath = Path.Combine(uploadDir, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/Uploads/{subFolder}/{uniqueFileName}";
        }

        #endregion
    }
}
