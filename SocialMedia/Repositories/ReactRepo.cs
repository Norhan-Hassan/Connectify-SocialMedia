using SocialMedia.Data;
using SocialMedia.Models;
using SocialMedia.Repos;

namespace SocialMedia.Repositories
{
    public class ReactRepo : IReactRepo
    {
        private readonly ApplicationDbContext context;
        private readonly IApplicationUserRepo userRepo;
        private readonly IPostRepo postRepo;

        public ReactRepo(ApplicationDbContext context, IApplicationUserRepo userRepo, IPostRepo postRepo)
        {
            this.context = context;
            this.userRepo = userRepo;
            this.postRepo = postRepo;
        }

        public void AddReact(Reaction reaction)
        {
            context.Add(reaction);
        }

        public void DeleteReact(Reaction reaction)
        {
            context.Remove(reaction);
        }

        public async Task<int> GetReactCount(int postId)
        {
            var post = await postRepo.GetPostAync(postId);
            int count = 0;
            var reactions = post.Reactions;
            if (reactions.Any())
            {
                foreach (var react in reactions)
                {
                    count++;
                }
            }
            return count;
        }
        public async Task<int> SaveAllChangesAsync()
        {
            return await context.SaveChangesAsync();
        }

    }
}
