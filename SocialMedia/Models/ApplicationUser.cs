using Microsoft.AspNetCore.Identity;

namespace SocialMedia.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Country { get; set; }
        public string City { get; set; }

        public string? Bio { get; set; }

        public string? KnownAs { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public Gender Gender { get; set; }
        public ICollection<Photo> Photos { get; set; }


        //public int GetUserAge()
        //{
        //    return DateOfBirth.CalculateAge();
        //}
    }
}
