using Microsoft.EntityFrameworkCore;
using Midia.Domain.Entities;

namespace Midia.Infrastructure.Data
{
    public class MediaDbContext : DbContext
    {
        public MediaDbContext(DbContextOptions<MediaDbContext> options) : base(options) { }

        public DbSet<Media> Midias { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Media>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome)
                      .IsRequired()
                      .HasMaxLength(200);
                entity.Property(e => e.Descricao)
                      .HasMaxLength(1000);
                entity.Property(e => e.UrlMidia)
                      .IsRequired()
                      .HasMaxLength(500);
            });

            modelBuilder.Entity<Media>().HasData(
                new
                {
                    Id = -1,
                    Nome = "Música A",
                    Descricao = "Descrição A",
                    UrlMidia = "https://teste.com/musicaA.mp3"
                },
                new
                {
                    Id = -2,
                    Nome = "Música B",
                    Descricao = "Descrição B",
                    UrlMidia = "https://teste.com/musicaB.mp3"
                }
            );
        }
    }
}
