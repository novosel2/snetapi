using Core.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostReaction> PostReactions { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentReaction> CommentReactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>()
                .HasOne(p => p.UserProfile)
                .WithMany()
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<PostReaction>()
                .HasKey(pr => new 
                {
                    pr.PostId,
                    pr.UserId
                });

            modelBuilder.Entity<PostReaction>()
                .HasOne(pr => pr.Post)
                .WithMany(p => p.Reactions)
                .HasForeignKey(pr => pr.PostId);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.UserProfile)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CommentReaction>()
                .HasKey(cr => new
                {
                    cr.CommentId,
                    cr.UserId
                });

            modelBuilder.Entity<CommentReaction>()
                .HasOne(cr => cr.Comment)
                .WithMany(c => c.Reactions)
                .HasForeignKey(cr => cr.CommentId);
        }
    }
}
