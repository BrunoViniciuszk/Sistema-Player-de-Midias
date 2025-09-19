using api_dotnet.Storage.Strategy.Interfaces;

namespace api_dotnet.Storage.Strategy.Implementations
{
    public class LocalStorageStrategy : IStorageStrategy
    {
        private readonly string _uploadFolder = "Uploads/Imagens";


        public async Task<string> SaveAsync(IFormFile file, string fileName)
        {
            if (!Directory.Exists(_uploadFolder))
                Directory.CreateDirectory(_uploadFolder);

            var path = Path.Combine(_uploadFolder, fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

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
