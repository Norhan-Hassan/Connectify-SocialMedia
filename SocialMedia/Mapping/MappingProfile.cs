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

            #region Photo Mapping
            CreateMap<Photo, PhotoDto>();
            #endregion

            #region Message Mapping
            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.senderPhotoUrl, opt => opt.MapFrom(src => src.SenderUser.Photos.FirstOrDefault(p => p.IsMainPhoto == true).PhotoUrl))
                .ForMember(dest => dest.senderUserName, opt => opt.MapFrom(src => src.SenderUser.UserName))
               .ForMember(dest => dest.receiverPhotoUrl, opt => opt.MapFrom(src => src.ReceiverUser.Photos.FirstOrDefault(p => p.IsMainPhoto == true).PhotoUrl))
               .ForMember(dest => dest.receiverUserName, opt => opt.MapFrom(src => src.ReceiverUser.UserName));
            #endregion

            #region Post Mapping
            CreateMap<Post, PostDto>()
                .ForMember(dest => dest.IsAnonymous, opt => opt.MapFrom(src => src.applicationUser.UserName == null ? true : false))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.applicationUser.UserName == null ? "Anonymous" : src.applicationUser.UserName))
                .ReverseMap();
            #endregion
        }

    }
}
