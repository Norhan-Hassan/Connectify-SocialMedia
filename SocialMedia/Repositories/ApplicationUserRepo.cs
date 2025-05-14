using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Data;
using SocialMedia.DTOs;
using SocialMedia.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialMedia.Repositories
{
    public class ApplicationUserRepo : IApplicationUserRepo
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configure;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        public ApplicationUserRepo(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration configure, IMapper mapper)
        {
            _userManager = userManager;
            _configure = configure;
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersAsync()
        {
            var users = await _context.ApplicationUsers.ToListAsync();
            return users;
        }

        public async Task<ApplicationUser> GetUserById(int id)
        {
            return await _context.ApplicationUsers.FindAsync(id);
        }

        public async Task<ApplicationUser> GetUserByName(string name)
        {
            return await _context.ApplicationUsers.SingleOrDefaultAsync(u => u.UserName == name);
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

                    //generate dynamic token for every request according to using guid
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
    }
}
