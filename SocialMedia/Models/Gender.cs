using System.Runtime.Serialization;

namespace SocialMedia.Models
{
    public enum Gender
    {
        [EnumMember(Value = "Male")]
        Male,
        [EnumMember(Value = "Female")]
        Female
    }
}
