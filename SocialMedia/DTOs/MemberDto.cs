using SocialMedia.Models;

namespace SocialMedia.DTOs
{
    public class MemberDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? Address { get; set; }
        public string?  Bio { get; set; }
        public int Age { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastActive { get; set; }
        public string Gender { get; set; }

        public ICollection<PhotoDto> Photos { get; set; }

    }
}
