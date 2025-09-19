using Xunit;
using api_dotnet.Data;
using api_dotnet.Models;
using api_dotnet.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

public class MidiaRepositoryTests
{
    private AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_AddsMidia()
    {
        using var context = CreateDbContext();
        var repo = new MidiaRepository(context);

        var midia = new Midia { Nome = "Teste" };
        await repo.CreateAsync(midia);

        Assert.Equal(1, context.Midias.Count());
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllMidias()
    {
        using var context = CreateDbContext();
        context.Midias.Add(new Midia { Nome = "M1" });
        context.Midias.Add(new Midia { Nome = "M2" });
        await context.SaveChangesAsync();

        var repo = new MidiaRepository(context);
        var result = await repo.GetAllAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsCorrectMidia()
    {
        using var context = CreateDbContext();
        var midia = new Midia { Nome = "Buscar" };
        context.Midias.Add(midia);
        await context.SaveChangesAsync();

        var repo = new MidiaRepository(context);
        var result = await repo.GetByIdAsync(midia.Id);

        Assert.NotNull(result);
        Assert.Equal("Buscar", result.Nome);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesMidia()
    {
        using var context = CreateDbContext();
        var midia = new Midia { Nome = "Antigo" };
        context.Midias.Add(midia);
        await context.SaveChangesAsync();

        var repo = new MidiaRepository(context);
        midia.Nome = "Novo";
        await repo.UpdateAsync(midia);

        Assert.Equal("Novo", context.Midias.First().Nome);
    }

    [Fact]
    public async Task DeleteAsync_RemovesMidia()
    {
        using var context = CreateDbContext();
        var midia = new Midia { Nome = "Excluir" };
        context.Midias.Add(midia);
        await context.SaveChangesAsync();

        var repo = new MidiaRepository(context);
        await repo.DeleteAsync(midia);

        Assert.Empty(context.Midias);
    }

    [Fact]
    public async Task RemoveFromPlaylistsAsync_RemovesAllRelations()
    {
        using var context = CreateDbContext();
        var midia = new Midia { Nome = "ComPlaylist" };
        var playlist = new Playlist { Nome = "Teste" };
        var relation = new MidiaPlaylist { Midia = midia, Playlist = playlist };
        context.Midias.Add(midia);
        context.Playlists.Add(playlist);
        context.MidiaPlaylists.Add(relation);
        await context.SaveChangesAsync();

        var repo = new MidiaRepository(context);
        await repo.RemoveFromPlaylistsAsync(midia.Id);

        Assert.Empty(context.MidiaPlaylists);
    }
}
