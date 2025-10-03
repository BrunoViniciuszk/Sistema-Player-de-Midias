namespace Auth.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IAppUserRepository AppUsers { get; }
        Task<int> CommitAsync();
    }
}
