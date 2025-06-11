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
        public ICollection<UserPoke> PokedByUsers { get; set; }
        public ICollection<UserPoke> PokedUsers { get; set; }
        public ICollection<Message> MessagesSent { get; set; }
        public ICollection<Message> MessagesReceived { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<Reaction> Reactions { get; set; }

        public ICollection<UserFollow> Followees { get; set; }
        public ICollection<UserFollow> Followers { get; set; }
    }
}
