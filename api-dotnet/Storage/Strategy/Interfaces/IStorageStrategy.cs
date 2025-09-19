namespace api_dotnet.Storage.Strategy.Interfaces
{
    public interface IStorageStrategy
    {
        Task<String> SaveAsync(IFormFile file, string fileName);
        Task DeleteAsync(string path);
    }
}
