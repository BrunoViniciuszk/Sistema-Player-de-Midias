using Xunit;
using AutoMapper;
using api_dotnet.Mappings;
using api_dotnet.Models;
using api_dotnet.Models.Dtos;
using System.Collections.Generic;

public class MappingProfileTests
{
    private readonly IMapper _mapper;

    public MappingProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        config.AssertConfigurationIsValid(); 

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Playlist_To_PlaylistDto_MapsCorrectly()
    {
        
        var playlist = new Playlist
        {
            Id = 1,
            Nome = "Favoritas",
            Midias = new List<MidiaPlaylist>
            {
                new MidiaPlaylist
                {
                    Midia = new Midia { Id = 10, Nome = "Música A", Descricao = "Desc A", UrlMidia = "urlA" },
                    ExibirNoPlayer = true
                }
            }
        };

        
        var dto = _mapper.Map<PlaylistDto>(playlist);

        
        Assert.Equal(1, dto.Id);
        Assert.Equal("Favoritas", dto.Nome);
        Assert.Single(dto.Midias);
        Assert.Equal(10, dto.Midias[0].Id);
        Assert.True(dto.Midias[0].ExibirNoPlayer);
    }

    [Fact]
    public void Playlist_To_PlaylistNomeDto_MapsCorrectly()
    {
        var playlist = new Playlist { Id = 2, Nome = "Trilha Sonora" };

        var dto = _mapper.Map<PlaylistNomeDto>(playlist);

        Assert.Equal(2, dto.Id);
        Assert.Equal("Trilha Sonora", dto.Nome);
    }

    [Fact]
    public void MidiaPlaylist_To_MidiaDto_MapsCorrectly()
    {
        var relation = new MidiaPlaylist
        {
            PlaylistId = 1,
            MidiaId = 5,
            ExibirNoPlayer = false,
            Midia = new Midia { Id = 5, Nome = "Video X", Descricao = "Um vídeo", UrlMidia = "urlX" }
        };

        var dto = _mapper.Map<MidiaDto>(relation);

        Assert.Equal(5, dto.Id);
        Assert.Equal("Video X", dto.Nome);
        Assert.Equal("Um vídeo", dto.Descricao);
        Assert.Equal("urlX", dto.UrlMidia);
        Assert.False(dto.ExibirNoPlayer);
    }
}
