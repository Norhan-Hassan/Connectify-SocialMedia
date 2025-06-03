namespace SocialMedia.DTOs
{
    public class PostDto
    {
        public string Content { get; set; }
        public string? UserName { get; set; }
        public bool IsAnonymous { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
