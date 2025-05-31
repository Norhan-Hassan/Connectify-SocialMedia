using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMedia.Models
{
    public class Message
    {
        public int ID { get; set; }

        public string Content { get; set; }

        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReadAt { get; set; }

        public bool SenderDeleted { get; set; }
        public bool ReceiverDeleted { get; set; }

        [ForeignKey("SenderUser")]
        public string senderUserId { get; set; }
        public ApplicationUser SenderUser { get; set; }


        [ForeignKey("RecieverUser")]
        public string receiverUserId { get; set; }
        public ApplicationUser ReceiverUser { get; set; }
    }
}
