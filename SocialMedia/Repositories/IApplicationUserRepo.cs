using Microsoft.AspNetCore.Identity;
using SocialMedia.DTOs;
using SocialMedia.Models;

namespace SocialMedia.Repositories
{
    public interface IApplicationUserRepo
    {

        Task<IdentityResult> Register(RegisterDto registerDto);
        Task<LoginResponse> Login(LoginDto loginDto);
        Task LogOut();
        Task<ApplicationUser> GetUserByIdAsync(int id);
        Task<ApplicationUser> GetUserByNameAsync(string name);
        Task<MemberDto> GetMemberByNameAsync(string name);
        Task<IEnumerable<MemberDto>> GetMembersAsync();

        void Update(ApplicationUser user);

        Task<int> SaveAllChangesAsync();
    }
}
