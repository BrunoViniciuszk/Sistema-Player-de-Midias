namespace Midia.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IMediaRepository Medias { get; }
        Task<int> CommitAsync(CancellationToken cancellationToken = default);
    }
}
