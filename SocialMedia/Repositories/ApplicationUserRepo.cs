using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Data;
using SocialMedia.DTOs;
using SocialMedia.Helpers;
using SocialMedia.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialMedia.Repositories
{
    public class ApplicationUserRepo : IApplicationUserRepo
    {
        #region fields
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configure;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        #endregion

        #region constructor
        public ApplicationUserRepo(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configure, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configure = configure;
            _mapper = mapper;
            _context = context;

        }

        #endregion

        #region Methods
        public PagedList<MemberDto> GetMembers(UserParams userParams)
        {
            var users = _context.ApplicationUsers.Include(u => u.Photos).AsNoTracking().AsQueryable();
            var otherUsers = users.Where(u => u.UserName != userParams.CurrentUserName).ToList();
            var mappedUsers = _mapper.Map<IEnumerable<MemberDto>>(otherUsers);
            return PagedList<MemberDto>.Create(mappedUsers, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            return await _context.ApplicationUsers.Include(u => u.Photos).SingleOrDefaultAsync();
        }

        public async Task<MemberDto> GetMemberByNameAsync(string name)
        {
            var user = await _context.ApplicationUsers.Include(u => u.Photos).SingleOrDefaultAsync(u => u.UserName == name);
            var mappedUser = _mapper.Map<MemberDto>(user);
            return mappedUser;
        }
        public async Task<ApplicationUser> GetUserByNameAsync(string name)
        {
            var user = await _context.ApplicationUsers.Include(u => u.Photos).SingleOrDefaultAsync(u => u.UserName == name);

            return user;
        }

        public async Task<LoginResponse> Login(LoginDto loginDto)
        {
            ApplicationUser applicationUser = await _userManager.FindByNameAsync(loginDto.Name);
            if (applicationUser != null)
            {
                bool found = await _userManager.CheckPasswordAsync(applicationUser, loginDto.Password);
                if (found)
                {
                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, applicationUser.Id));
                    claims.Add(new Claim(ClaimTypes.Name, applicationUser.UserName));


                    claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));



                    SymmetricSecurityKey signkey = new SymmetricSecurityKey(
                       Encoding.UTF8.GetBytes(_configure["JWT:key"]));

                    SigningCredentials signingCredentials = new SigningCredentials(signkey, SecurityAlgorithms.HmacSha256);


                    JwtSecurityToken token = new JwtSecurityToken(
                        issuer: _configure["JWT:issuer"],
                        audience: _configure["JWT:audience"],
                        expires: DateTime.Now.AddHours(2),
                        claims: claims,
                        signingCredentials: signingCredentials
                    );
                    return new LoginResponse()
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        Expired = token.ValidTo
                    };
                }

            }
            return null;
        }

        public async Task<IdentityResult> Register(RegisterDto registerDto)
        {
            var userInDb = await _userManager.FindByNameAsync(registerDto.Name);
            if (userInDb == null)
            {
                var applicationUser = _mapper.Map<ApplicationUser>(registerDto);

                IdentityResult result = await _userManager.CreateAsync(applicationUser, registerDto.Password);

                return result;

            }
            else
                return IdentityResult.Failed(new IdentityError { Description = "User Name already exists" });

        }

        public void Update(ApplicationUser user)
        {
            _context.Entry<ApplicationUser>(user).State = EntityState.Modified;
        }

        public async Task LogOut()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<int> SaveAllChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        #endregion
    }
}
