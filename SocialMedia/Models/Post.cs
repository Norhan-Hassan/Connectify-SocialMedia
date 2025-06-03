using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMedia.Models
{
    public class Post
    {
        [Key]
        public int ID { get; set; }
        public string Content { get; set; }
        public bool IsAnonymous { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        [ForeignKey(nameof(applicationUser))]
        public string applicationUserId { get; set; }
        public ApplicationUser applicationUser { get; set; }
    }
}
