namespace SocialMedia.Services
{
    public interface IPresenceTracker
    {
        Task<IReadOnlyList<string>> GetConnectionForUser(string userId);
    }
}