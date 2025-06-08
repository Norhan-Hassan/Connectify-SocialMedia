using SocialMedia.Models;
using System.Text.Json.Serialization;

namespace SocialMedia.DTOs
{
    public class AddReactDto
    {
        public ReactionType Type { get; set; }
        [JsonIgnore]
        public DateTime ReactedAt { get; set; }
    }
}
