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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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

            modelBuilder.Entity<Friendship>()
                .HasOne(fs => fs.SenderUser)
                .WithMany(u => u.FriendsAsSender)
                .HasForeignKey(fs => fs.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friendship>()
                .HasOne(fs => fs.ReceiverUser)
                .WithMany(u => u.FriendsAsReciever)
                .HasForeignKey(fs => fs.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FriendRequest>()
                .HasOne(fs => fs.SenderUser)
                .WithMany(u => u.FriendRequestsAsSender)
                .HasForeignKey(fs => fs.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FriendRequest>()
                .HasOne(fs => fs.ReceiverUser)
                .WithMany(u => u.FriendRequestsAsReciever)
                .HasForeignKey(fs => fs.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Post>()
                .HasOne(p => p.UserProfile)
                .WithMany()
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<FileUrl>()
                .HasKey(fu => new
                {
                    fu.PostId,
                    fu.Url
                });

            modelBuilder.Entity<FileUrl>()
                .HasOne(fu => fu.Post)
                .WithMany(p => p.FileUrls)
                .HasForeignKey(fu => fu.PostId);

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
                .HasOne(c => c.ParentComment)
                .WithMany(pr => pr.CommentReplies)
                .HasForeignKey(pr => pr.ParentCommentId);

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
