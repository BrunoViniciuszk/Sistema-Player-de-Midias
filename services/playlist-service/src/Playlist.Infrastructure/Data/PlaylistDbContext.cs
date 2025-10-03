using Microsoft.EntityFrameworkCore;
using Playlist.Domain.Entities;

namespace Playlist.Infrastructure.Data
{
    public class PlaylistDbContext : DbContext
    {
        public PlaylistDbContext(DbContextOptions<PlaylistDbContext> options) : base(options) { }

        public DbSet<PlaylistEntity> Playlists { get; set; } = null!;
        public DbSet<MidiaPlaylist> MidiaPlaylists { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MidiaPlaylist>()
                .HasKey(mp => new { mp.PlaylistId, mp.MidiaId });

         
            modelBuilder.Entity<MidiaPlaylist>()
                .HasOne(mp => mp.Playlist)
                .WithMany(p => p.Midias)
                .HasForeignKey(mp => mp.PlaylistId);

         
            modelBuilder.Entity<MidiaPlaylist>().HasData(
                new { PlaylistId = 1, MidiaId = 1, ExibirNoPlayer = false },
                new { PlaylistId = 1, MidiaId = 2, ExibirNoPlayer = false }
            );
        }
    }
}
