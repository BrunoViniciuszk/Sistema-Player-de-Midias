using api_dotnet.Data;
using api_dotnet.Repositories.Interfaces;

namespace api_dotnet.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IMidiaRepository Midias { get; }
        public IPlaylistRepository Playlists{ get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Midias = new MidiaRepository(_context);
            Playlists = new PlaylistRepository(_context);
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
