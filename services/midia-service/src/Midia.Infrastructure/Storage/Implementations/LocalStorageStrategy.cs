using Microsoft.Extensions.Configuration;
using Midia.Application.Interfaces;

namespace Midia.Infrastructure.Storage.Implementations
{
    public class LocalStorageStrategy : IStorageStrategy
    {
        private readonly string _uploadFolder;

        public LocalStorageStrategy(IConfiguration configuration)
        {
            _uploadFolder = configuration["Storage:Local:Path"] ?? "storage/uploads";
        }

        public async Task<string> SaveAsync(Stream fileStream, string fileName, string contentType)
        {
            if (!Directory.Exists(_uploadFolder))
                Directory.CreateDirectory(_uploadFolder);

            var path = Path.Combine(_uploadFolder, fileName);

            using var stream = new FileStream(path, FileMode.Create, FileAccess.Write);
            await fileStream.CopyToAsync(stream);

            return path;
        }

        public Task DeleteAsync(string path)
        {
            if (File.Exists(path))
                File.Delete(path);

            return Task.CompletedTask;
        }
    }
}
