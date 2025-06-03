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
        public DbSet<Message> Messages { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Post> Posts { get; set; }

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

            builder.Entity<Message>().HasOne(u => u.ReceiverUser)
                                     .WithMany(u => u.MessagesReceived)
                                     .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>().HasOne(u => u.SenderUser)
                                     .WithMany(u => u.MessagesSent)
                                     .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
