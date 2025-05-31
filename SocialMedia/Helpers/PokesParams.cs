namespace SocialMedia.Helpers
{
    public class PokesParams : PaginationParams
    {
        public string? UserId { get; set; }
        public string Predicate { get; set; }
    }
}
