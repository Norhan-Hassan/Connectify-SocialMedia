using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMedia.Models
{
    public class ApplicationUserConnection
    {
        [Key]
        public string ConnectionID { get; set; }


        [ForeignKey(nameof(applicationUser))]
        public string ApplicationUserId { get; set; }
        public ApplicationUser applicationUser { get; set; }


        [ForeignKey(nameof(Group))]
        public int GroupID { get; set; }
        public Group Group { get; set; }
    }
}
