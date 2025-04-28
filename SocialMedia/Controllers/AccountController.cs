using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Data;
using SocialMedia.DTOs;
using SocialMedia.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialMedia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configure;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration configure, IMapper mapper)
        {
            _userManager = userManager;
            _configure = configure;
            _mapper = mapper;
            _context = context;

        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
                        
            var userInDb = _context.ApplicationUsers.FirstOrDefault(u => u.UserName==registerDto.Name);
            if (userInDb != null)
            { 
                return UnprocessableEntity("This Name is registered before, use another User Name");
            }


            //var applicationUser = _mapper.Map<ApplicationUser>(registerDto);

            var applicationUser = new ApplicationUser();
            applicationUser.Email = registerDto.Email;
            applicationUser.UserName = registerDto.Name;
            applicationUser.Gender = registerDto.Gender;
            applicationUser.CreatedAt = DateTime.Now;

            IdentityResult result = await _userManager.CreateAsync(applicationUser, registerDto.Password);
            if (result.Succeeded)
            {
                return CreatedAtAction(nameof(Register), new { id = applicationUser.Id });
            }
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", result.Errors.ToString());
            } 
            return BadRequest("Error in Register");
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginRequest)
        {

            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = await _userManager.FindByNameAsync(loginRequest.Name);
                if (applicationUser != null)
                {
                    bool found = await _userManager.CheckPasswordAsync(applicationUser, loginRequest.Password);
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
                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expired = token.ValidTo
                        });
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
            }
            return BadRequest("User Name or password is invalid");
        }
    }
    
}
