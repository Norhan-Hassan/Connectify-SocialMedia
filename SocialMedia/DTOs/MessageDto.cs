using System.Text.Json.Serialization;

namespace SocialMedia.DTOs
{
    public class MessageDto
    {
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
        public DateTime? ReadAt { get; set; }
        public string senderUserId { get; set; }
        public string senderUserName { get; set; }

        public string senderPhotoUrl { get; set; }
        public string receiverUserId { get; set; }
        public string receiverUserName { get; set; }

        public string receiverPhotoUrl { get; set; }
        [JsonIgnore]
        public bool SenderDeleted { get; set; }
        [JsonIgnore]
        public bool ReceiverDeleted { get; set; }


    }
}
