using AutoMapper;
using Microsoft.OpenApi.Extensions;
using SocialMedia.DTOs;
using SocialMedia.Extensions;
using SocialMedia.Models;

namespace SocialMedia.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region ApplicationUser Mapping

            CreateMap<ApplicationUser, RegisterDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UserName))
                .ReverseMap();
            CreateMap<ApplicationUser, MemberDto>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.GetDisplayName()))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()))
                .ForMember(dest => dest.MainPhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMainPhoto == true).PhotoUrl));

            CreateMap<MemberUpdateDto, ApplicationUser>();

            #endregion

            CreateMap<Photo, PhotoDto>();
        }
    }
}
