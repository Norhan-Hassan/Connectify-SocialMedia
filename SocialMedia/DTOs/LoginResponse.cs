namespace SocialMedia.DTOs
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public DateTime Expired { get; set; }
    }
}
