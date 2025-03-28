﻿using Core.Data.Entities;
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
        public DbSet<FileUrl> FileUrls { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Follow> Follows { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region KEYS

            modelBuilder.Entity<FileUrl>()
                .HasKey(fu => new
                {
                    fu.PostId,
                    fu.Url
                });

            modelBuilder.Entity<PostReaction>()
                .HasKey(pr => new
                {
                    pr.PostId,
                    pr.UserId
                });

            modelBuilder.Entity<CommentReaction>()
                .HasKey(cr => new
                {
                    cr.CommentId,
                    cr.UserId
                });

            #endregion

            #region INDEXES

            modelBuilder.Entity<Profile>()
                .HasIndex(p => p.Id);

            modelBuilder.Entity<Profile>()
                .HasIndex(p => p.Username);

            modelBuilder.Entity<Profile>()
                .HasIndex(p => p.FirstName);

            modelBuilder.Entity<Profile>()
                .HasIndex(p => p.LastName);

            modelBuilder.Entity<Profile>()
                .HasIndex(p => new 
                { 
                    p.Username, 
                    p.FirstName, 
                    p.LastName
                });

            modelBuilder.Entity<Follow>()
                .HasIndex(f => new
                {
                    f.FollowerId,
                    f.FollowedId
                })
                .IsUnique();

            modelBuilder.Entity<FriendRequest>()
                .HasIndex(fr => new
                {
                    fr.SenderId,
                    fr.ReceiverId
                })
                .IsUnique();

            modelBuilder.Entity<Friendship>()
                .HasIndex(fs => new
                {
                    fs.SenderId,
                    fs.ReceiverId
                })
                .IsUnique();

            modelBuilder.Entity<Post>()
                .HasIndex(p => p.Id);

            modelBuilder.Entity<Post>()
                .HasIndex(p => p.UserId);

            modelBuilder.Entity<Post>()
                .HasIndex(p => p.CreatedOn);

            modelBuilder.Entity<Post>()
                .HasIndex(p => p.PopularityScore);

            modelBuilder.Entity<Post>()
                .HasIndex(p => new
                {
                    p.CreatedOn,
                    p.PopularityScore,
                    p.UserId
                });

            modelBuilder.Entity<PostReaction>()
                .HasIndex(pr => new
                {
                    pr.PostId,
                    pr.CreatedOn
                });

            modelBuilder.Entity<PostReaction>()
                .HasIndex(pr => new
                {
                    pr.PostId,
                    pr.UserId
                });

            modelBuilder.Entity<FileUrl>()
                .HasIndex(f => f.PostId);


            modelBuilder.Entity<Comment>()
                .HasIndex(c => new
                {
                    c.UserId,
                    c.PostId,
                    c.CreatedOn
                });

            #endregion

            #region RELATIONS

            modelBuilder.Entity<Follow>()
                .HasOne(f => f.Follower)
                .WithMany(p => p.Following)
                .HasForeignKey(f => f.FollowerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Follow>()
                .HasOne(f => f.Followed)
                .WithMany(p => p.Followers)
                .HasForeignKey(f => f.FollowedId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Friendship>()
                .HasOne(fs => fs.SenderUser)
                .WithMany(u => u.FriendsAsSender)
                .HasForeignKey(fs => fs.SenderId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Friendship>()
                .HasOne(fs => fs.ReceiverUser)
                .WithMany(u => u.FriendsAsReceiver)
                .HasForeignKey(fs => fs.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<FriendRequest>()
                .HasOne(fs => fs.SenderUser)
                .WithMany(u => u.FriendRequestsAsSender)
                .HasForeignKey(fs => fs.SenderId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<FriendRequest>()
                .HasOne(fs => fs.ReceiverUser)
                .WithMany(u => u.FriendRequestsAsReceiver)
                .HasForeignKey(fs => fs.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<FileUrl>()
                .HasOne(fu => fu.Post)
                .WithMany(p => p.FileUrls)
                .HasForeignKey(fu => fu.PostId);

            modelBuilder.Entity<PostReaction>()
                .HasOne(pr => pr.Post)
                .WithMany(p => p.Reactions)
                .HasForeignKey(pr => pr.PostId);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.ParentComment)
                .WithMany(pr => pr.CommentReplies)
                .HasForeignKey(pr => pr.ParentCommentId);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CommentReaction>()
                .HasOne(cr => cr.Comment)
                .WithMany(c => c.Reactions)
                .HasForeignKey(cr => cr.CommentId);

            #endregion
        }
    }
}
