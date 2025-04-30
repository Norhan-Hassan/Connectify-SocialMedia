using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.DTOs;
using SocialMedia.Helpers;

namespace SocialMedia.Repositories
{
    public interface IApplicationUserRepo
    {

        Task<IdentityResult> Register(RegisterDto registerDto);
        Task<LoginResponse> Login(LoginDto loginDto);
    }
}
