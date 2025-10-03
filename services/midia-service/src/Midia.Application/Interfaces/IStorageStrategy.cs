namespace Midia.Application.Interfaces
{
    public interface IStorageStrategy
    {
        Task<string> SaveAsync(Stream fileStream, string fileName, string contentType);
        Task DeleteAsync(string path);

    }
}
