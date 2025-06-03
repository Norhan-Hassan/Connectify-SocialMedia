using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Models
{
    public class Group
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }

        public ICollection<ApplicationUserConnection> Connections { get; set; }
    }
}
