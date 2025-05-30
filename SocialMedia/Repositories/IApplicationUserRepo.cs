using Microsoft.AspNetCore.Identity;
using SocialMedia.DTOs;
using SocialMedia.Helpers;
using SocialMedia.Models;

namespace SocialMedia.Repositories
{
    public interface IApplicationUserRepo
    {

        Task<IdentityResult> Register(RegisterDto registerDto);
        Task<LoginResponse> Login(LoginDto loginDto);
        Task LogOut();
        Task<ApplicationUser> GetUserByIdAsync(string id);
        Task<ApplicationUser> GetUserByNameAsync(string name);
        Task<MemberDto> GetMemberByNameAsync(string name);
        PagedList<MemberDto> GetMembers(UserParams userParams);

        void Update(ApplicationUser user);

        Task<int> SaveAllChangesAsync();
    }
}
