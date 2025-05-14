using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.DTOs;
using SocialMedia.Models;
using System.Collections.Generic;

namespace SocialMedia.Repositories
{
    public interface IApplicationUserRepo
    {

        Task<IdentityResult> Register(RegisterDto registerDto);
        Task<LoginResponse> Login(LoginDto loginDto);

        Task<ApplicationUser> GetUserById(int id);
        Task<ApplicationUser> GetUserByName(string name);
        Task<IEnumerable<ApplicationUser>> GetUsersAsync();

        void Update(ApplicationUser user);
    }
}
