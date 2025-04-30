using AutoMapper;
using SocialMedia.DTOs;
using SocialMedia.Models;

namespace SocialMedia.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            #region ApplicationUser Mapping

            CreateMap<ApplicationUser, RegisterDto>()
                .ForMember(dest => dest.Name, src => src.MapFrom(src => src.UserName))
                .ReverseMap();

            #endregion

        }
    }
}
