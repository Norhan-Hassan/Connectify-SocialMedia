using System.Text.Json.Serialization;

namespace SocialMedia.DTOs
{
    public class UpdatePostDto
    {
        public string Content { get; set; }

        [JsonIgnore]
        public DateTime UpdatedAt { get; set; }
    }
}
