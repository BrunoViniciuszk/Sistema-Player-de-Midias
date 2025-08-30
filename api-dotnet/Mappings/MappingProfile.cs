using AutoMapper;
using api_dotnet.Models;
using api_dotnet.Models.Dtos;

namespace api_dotnet.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Playlist, PlaylistDto>()
                .ForMember(dest => dest.Midias, opt => opt.MapFrom(src => src.Midias));

            CreateMap<Playlist, PlaylistNomeDto>();

            CreateMap<MidiaPlaylist, MidiaDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Midia.Id))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Midia.Nome))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Midia.Descricao))
                .ForMember(dest => dest.UrlMidia, opt => opt.MapFrom(src => src.Midia.UrlMidia))
                .ForMember(dest => dest.ExibirNoPlayer, opt => opt.MapFrom(src => src.ExibirNoPlayer));
        }
    }
}
