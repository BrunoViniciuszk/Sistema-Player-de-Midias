using Xunit;
using api_dotnet.Data;
using api_dotnet.Models;
using api_dotnet.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

public class PlaylistRepositoryTests
{
    private AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_AddsPlaylist()
    {
        using var context = CreateDbContext();
        var repo = new PlaylistRepository(context);

        var playlist = new Playlist { Nome = "Nova Playlist" };
        await repo.CreateAsync(playlist);

        Assert.Equal(1, context.Playlists.Count());
    }

    [Fact]
    public async Task GetAllAsync_ReturnsPlaylists()
    {
        using var context = CreateDbContext();
        context.Playlists.Add(new Playlist { Nome = "P1" });
        context.Playlists.Add(new Playlist { Nome = "P2" });
        await context.SaveChangesAsync();

        var repo = new PlaylistRepository(context);
        var result = await repo.GetAllAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsPlaylist()
    {
        using var context = CreateDbContext();
        var playlist = new Playlist { Nome = "Buscar" };
        context.Playlists.Add(playlist);
        await context.SaveChangesAsync();

        var repo = new PlaylistRepository(context);
        var result = await repo.GetByIdAsync(playlist.Id);

        Assert.NotNull(result);
        Assert.Equal("Buscar", result.Nome);
    }

    [Fact]
    public async Task DeleteAsync_RemovesPlaylistAndRelations()
    {
        using var context = CreateDbContext();
        var playlist = new Playlist { Nome = "Excluir" };
        var midia = new Midia { Nome = "M1" };
        var relation = new MidiaPlaylist { Playlist = playlist, Midia = midia };
        context.Playlists.Add(playlist);
        context.Midias.Add(midia);
        context.MidiaPlaylists.Add(relation);
        await context.SaveChangesAsync();

        var repo = new PlaylistRepository(context);
        await repo.DeleteAsync(playlist);

        Assert.Empty(context.Playlists);
        Assert.Empty(context.MidiaPlaylists);
    }

    [Fact]
    public async Task AddMidiaPlaylistAsync_AddsRelation()
    {
        using var context = CreateDbContext();
        var repo = new PlaylistRepository(context);

        var relation = new MidiaPlaylist { PlaylistId = 1, MidiaId = 2 };
        await repo.AddMidiaPlaylistAsync(relation);

        Assert.Single(context.MidiaPlaylists);
    }

    [Fact]
    public async Task GetMidiaPlaylistAsync_ReturnsRelation()
    {
        using var context = CreateDbContext();
        var relation = new MidiaPlaylist { PlaylistId = 1, MidiaId = 2 };
        context.MidiaPlaylists.Add(relation);
        await context.SaveChangesAsync();

        var repo = new PlaylistRepository(context);
        var result = await repo.GetMidiaPlaylistAsync(1, 2);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task RemoveMidiaPlaylistAsync_RemovesRelation()
    {
        using var context = CreateDbContext();
        var relation = new MidiaPlaylist { PlaylistId = 1, MidiaId = 2 };
        context.MidiaPlaylists.Add(relation);
        await context.SaveChangesAsync();

        var repo = new PlaylistRepository(context);
        await repo.RemoveMidiaPlaylistAsync(relation);

        Assert.Empty(context.MidiaPlaylists);
    }

    [Fact]
    public async Task SaveChangesAsync_CommitsChanges()
    {
        using var context = CreateDbContext();
        var repo = new PlaylistRepository(context);

        context.Playlists.Add(new Playlist { Nome = "Salvar" });
        await repo.SaveChangesAsync();

        Assert.Equal(1, context.Playlists.Count());
    }
}
