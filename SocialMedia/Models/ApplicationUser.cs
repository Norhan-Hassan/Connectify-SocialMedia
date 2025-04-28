using Microsoft.AspNetCore.Identity;

namespace SocialMedia.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string? Address  { get; set; }
        public string? Bio {  get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; }
        public Gender Gender { get; set; }

        public string? ProfileImageUrl { get; set; }
    }
}
