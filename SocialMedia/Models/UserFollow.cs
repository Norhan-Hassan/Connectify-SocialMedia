using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMedia.Models
{
    public class UserFollow
    {
        [ForeignKey("Follower")]
        public string FollowerId { get; set; }
        public ApplicationUser Follower { get; set; }


        [ForeignKey("Followee")]
        public string FolloweeId { get; set; }
        public ApplicationUser Followee { get; set; }
    }
}
