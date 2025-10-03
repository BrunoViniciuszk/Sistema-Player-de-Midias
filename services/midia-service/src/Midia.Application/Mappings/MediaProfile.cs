using AutoMapper;
using Midia.Application.Dtos;
using Midia.Domain.Entities;

namespace Midia.Application.Mappings
{
    public class MediaProfile : Profile
    {
        public MediaProfile()
        {
            CreateMap<Media, MediaDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao))
                .ForMember(dest => dest.UrlMidia, opt => opt.MapFrom(src => src.UrlMidia))
                .ForMember(dest => dest.ExibirNoPlayer, opt => opt.Ignore()); 


            
            CreateMap<UploadMediaDto, Media>()
                .ForMember(dest => dest.UrlMidia, opt => opt.Ignore()); 
        }
    }
}
