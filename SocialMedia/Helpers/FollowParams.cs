namespace SocialMedia.Helpers
{
    public class FollowParams : PaginationParams
    {
        public string? UserId { get; set; }
        public string Predicate { get; set; }
    }
}
