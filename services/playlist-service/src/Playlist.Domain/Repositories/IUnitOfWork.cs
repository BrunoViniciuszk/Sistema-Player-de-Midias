namespace Playlist.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IPlaylistRepository Playlists { get; }
        Task<int> CommitAsync(CancellationToken ct = default);
    }
}
