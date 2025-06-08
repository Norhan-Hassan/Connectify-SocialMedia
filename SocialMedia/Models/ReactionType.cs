using System.Runtime.Serialization;

namespace SocialMedia.Models
{
    public enum ReactionType
    {
        [EnumMember(Value = "Like")]
        Like,
        [EnumMember(Value = "Love")]
        Love,
        [EnumMember(Value = "Care")]
        Care,
        [EnumMember(Value = "Haha")]
        Haha,
        [EnumMember(Value = "Angry")]
        Angry,
        [EnumMember(Value = "Sad")]
        Sad
    }
}
