using Auth.Domain.Entities;
using Auth.Application.Dtos;
using AutoMapper;

namespace Auth.Application.Mappings
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<AppUser, LoginResponseDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Token, opt => opt.Ignore()); 

        }
    }
}
