using api_dotnet.Data;
using api_dotnet.Models;
using api_dotnet.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api_dotnet.Services.Midias
{
    public class MidiaService : IMidiaService
    {
        private readonly IMidiaRepository _midiaRepository;
        private readonly IWebHostEnvironment _env;

        private static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png" };
        private static readonly string[] AllowedVideoExtensions = { ".mp4", ".avi", ".mov" };

        public MidiaService(IMidiaRepository midiaRepository, IWebHostEnvironment env)
        {
            _midiaRepository = midiaRepository;
            _env = env;
        }

        public async Task<IEnumerable<Midia>> GetAllAsync()
        {
            return await _midiaRepository.GetAllAsync();
        }


        public async Task<Midia> GetByIdAsync(int id)
        {
            var midia = await _midiaRepository.GetByIdAsync(id);
            if (midia == null) throw new KeyNotFoundException("Mídia não encontrada");
            return midia;
        }


        public async Task<Midia> UpdateAsync(int id, string? nome, string? descricao, IFormFile? file)
        {
            var existing = await _midiaRepository.GetByIdAsync(id);
            if (existing == null) throw new KeyNotFoundException("Mídia não encontrada");


            existing.Nome = nome ?? existing.Nome;
            existing.Descricao = descricao ?? existing.Descricao;


            if (file != null)
            {
                var ext = Path.GetExtension(file.FileName).ToLower();
                string subFolder = (ext == ".jpg" || ext == ".jpeg" || ext == ".png") ? "Imagens" : "Videos";

                var uploadDir = Path.Combine(_env.ContentRootPath, "Uploads", subFolder);
                if (!Directory.Exists(uploadDir)) Directory.CreateDirectory(uploadDir);

                var uniqueFileName = Guid.NewGuid() + ext;
                var filePath = Path.Combine(uploadDir, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }


                existing.UrlMidia = $"/Uploads/{subFolder}/{uniqueFileName}";
            }

            await _midiaRepository.UpdateAsync(existing);
            return existing;
        }


        public async Task DeleteAsync(int id)
        {
            var midia = await _midiaRepository.GetByIdAsync(id);
            if (midia == null) throw new KeyNotFoundException("Midia não encontrada");


            await _midiaRepository.RemoveFromPlaylistsAsync(id);


            if (!string.IsNullOrEmpty(midia.UrlMidia))
            {
                var filePath = Path.Combine(
                    _env.ContentRootPath,
                    midia.UrlMidia.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString())
                );
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

            await _midiaRepository.DeleteAsync(midia);
        }


        public async Task<Midia> UploadAndCreateMidiaAsync(IFormFile file, string nome, string descricao)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Arquivo inválido");

            var ext = Path.GetExtension(file.FileName).ToLower();
            if (!AllowedImageExtensions.Contains(ext) && !AllowedVideoExtensions.Contains(ext))
                throw new ArgumentException("Formato de arquivo não suportado");

            string subFolder = AllowedImageExtensions.Contains(ext) ? "Imagens" : "Videos";

            var uploadDir = Path.Combine(_env.ContentRootPath, "Uploads", subFolder);
            if (!Directory.Exists(uploadDir)) Directory.CreateDirectory(uploadDir);

            var uniqueFileName = Guid.NewGuid().ToString() + ext;
            var filePath = Path.Combine(uploadDir, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var url = $"/Uploads/{subFolder}/{uniqueFileName}";

            var midia = new Midia
            {
                Nome = nome,
                Descricao = descricao,
                UrlMidia = url
            };

            return await _midiaRepository.CreateAsync(midia);
        }

    }
}
