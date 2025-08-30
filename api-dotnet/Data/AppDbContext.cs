using api_dotnet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api_dotnet.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<Midia> Midias { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<MidiaPlaylist> MidiaPlaylists { get; set; }
        public DbSet<AppUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MidiaPlaylist>()
                .HasKey(mp => new { mp.PlaylistId, mp.MidiaId});

            modelBuilder.Entity<MidiaPlaylist>()
                .HasOne(mp => mp.Playlist)
                .WithMany(m => m.Midias)
                .HasForeignKey(mp => mp.PlaylistId);

            modelBuilder.Entity<MidiaPlaylist>()
                .HasOne(mp => mp.Midia)
                .WithMany(p => p.Playlists)
                .HasForeignKey(mp => mp.MidiaId);

            modelBuilder.Entity<Playlist>().HasData(
            new Playlist { Id = 1, Nome = "Favoritas" },
            new Playlist { Id = 2, Nome = "Trilha Sonora" }
            );

            
            modelBuilder.Entity<Midia>().HasData(
                new Midia { Id = 1, Nome = "Música A", UrlMidia = "https://teste.com/musicaA.mp3" },
                new Midia { Id = 2, Nome = "Música B", UrlMidia = "https://teste.com/musicaB.mp3" }
            );

            
            modelBuilder.Entity<MidiaPlaylist>().HasData(
                new MidiaPlaylist { PlaylistId = 1, MidiaId = 1, ExibirNoPlayer = false },
                new MidiaPlaylist { PlaylistId = 1, MidiaId = 2, ExibirNoPlayer = false }
            );
        }
    }
}
