using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMedia.Models
{
    public class Photo
    {
        [Key]
        public int ID { get; set; }
        public int PublicId { get; set; }
        public string PhotoUrl { get; set; }
        public string? Description {  get; set; }
        public bool IsMainPhoto { get; set; }


        [ForeignKey(nameof(applicationUser))]
        public string ApplicationUserId { get; set; }
        public ApplicationUser applicationUser { get; set; }
    }
}
