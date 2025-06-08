using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMedia.Models
{
    public class Reaction
    {
        public int ID { get; set; }
        public ReactionType Type { get; set; }
        public DateTime ReactedAt { get; set; } = DateTime.UtcNow;


        [ForeignKey(nameof(applicationUser))]
        public string applicationUserId { get; set; }
        public ApplicationUser applicationUser { get; set; }


        [ForeignKey(nameof(post))]
        public int postId { get; set; }
        public Post post { get; set; }

    }
}
