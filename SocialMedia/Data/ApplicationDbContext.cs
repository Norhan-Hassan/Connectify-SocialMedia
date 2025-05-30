using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Models;
namespace SocialMedia.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext()
        {

        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationUserConnection> UserConnections { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<UserPoke> Pokes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserPoke>().HasKey(u => new { u.pokedUserId, u.sourceUserId });

            builder.Entity<UserPoke>().HasOne(s => s.SourceUser)
                                      .WithMany(d => d.PokedUsers)
                                      .HasForeignKey(s => s.sourceUserId)
                                      .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<UserPoke>().HasOne(s => s.PokedUser)
                                      .WithMany(d => d.PokedByUsers)
                                      .HasForeignKey(s => s.pokedUserId)
                                      .OnDelete(DeleteBehavior.NoAction);



        }
    }
}
