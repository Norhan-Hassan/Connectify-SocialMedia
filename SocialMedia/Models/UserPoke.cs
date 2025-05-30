using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMedia.Models
{
    public class UserPoke
    {
        [ForeignKey("SourceUser")]
        public string sourceUserId { get; set; }
        public ApplicationUser SourceUser { get; set; }


        [ForeignKey("PokedUser")]
        public string pokedUserId { get; set; }
        public ApplicationUser PokedUser { get; set; }
    }
}
