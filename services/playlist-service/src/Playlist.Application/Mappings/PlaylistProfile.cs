using api_dotnet.Models.Dtos;
using AutoMapper;
using Playlist.Application.Dtos;
using Playlist.Domain.Entities;

namespace Playlist.Application.Mappings
{
    public class PlaylistProfile : Profile
    {
        public PlaylistProfile()
        {
            
            CreateMap<MidiaPlaylist, MidiaDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.MidiaId))
                .ForMember(d => d.ExibirNoPlayer, opt => opt.MapFrom(src => src.ExibirNoPlayer))
                .ForMember(d => d.Nome, opt => opt.Ignore())
                .ForMember(d => d.Descricao, opt => opt.Ignore())
                .ForMember(d => d.UrlMidia, opt => opt.Ignore());

            CreateMap<PlaylistEntity, PlaylistDto>()
                .ForMember(d => d.Midias, opt => opt.MapFrom(src => src.Midias));

            CreateMap<PlaylistEntity, PlaylistNomeDto>();
        }
    }
}
