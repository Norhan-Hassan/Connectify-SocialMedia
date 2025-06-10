namespace SocialMedia.DTOs
{
    public class MemberProfileDto
    {
        public string UserName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string MainPhotoUrl { get; set; }
        public string? Bio { get; set; }

        public string? KnownAs { get; set; }
        public int Age { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastActive { get; set; }
        public string Gender { get; set; }
    }
}
