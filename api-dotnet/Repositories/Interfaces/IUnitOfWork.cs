namespace api_dotnet.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IMidiaRepository Midias { get; }
        IPlaylistRepository Playlists { get; }
        Task<int> CommitAsync();

    }
}
