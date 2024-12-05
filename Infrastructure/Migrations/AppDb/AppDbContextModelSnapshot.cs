﻿// <auto-generated />
using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations.AppDb
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Core.Data.Entities.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("ParentCommentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ParentCommentId");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments", (string)null);
                });

            modelBuilder.Entity("Core.Data.Entities.CommentReaction", b =>
                {
                    b.Property<Guid>("CommentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Reaction")
                        .HasColumnType("int");

                    b.HasKey("CommentId", "UserId");

                    b.ToTable("CommentReactions", (string)null);
                });

            modelBuilder.Entity("Core.Data.Entities.FileUrl", b =>
                {
                    b.Property<Guid>("PostId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("PostId", "Url");

                    b.ToTable("FileUrls", (string)null);
                });

            modelBuilder.Entity("Core.Data.Entities.Follow", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FollowedId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FollowerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("FollowedId");

                    b.HasIndex("FollowerId", "FollowedId")
                        .IsUnique();

                    b.ToTable("Follows", (string)null);
                });

            modelBuilder.Entity("Core.Data.Entities.FriendRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ReceiverId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SenderId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("SenderId", "ReceiverId")
                        .IsUnique();

                    b.ToTable("FriendRequests", (string)null);
                });

            modelBuilder.Entity("Core.Data.Entities.Friendship", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ReceiverId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SenderId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("SenderId", "ReceiverId")
                        .IsUnique();

                    b.ToTable("Friendships", (string)null);
                });

            modelBuilder.Entity("Core.Data.Entities.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("CommentCount")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Posts", (string)null);
                });

            modelBuilder.Entity("Core.Data.Entities.PostReaction", b =>
                {
                    b.Property<Guid>("PostId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Reaction")
                        .HasColumnType("int");

                    b.HasKey("PostId", "UserId");

                    b.ToTable("PostReactions", (string)null);
                });

            modelBuilder.Entity("Core.Data.Entities.Profile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("FollowersCount")
                        .HasColumnType("int");

                    b.Property<int>("FollowingCount")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PictureUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PreviousFollowers")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Username", "FirstName", "LastName");

                    b.ToTable("Profiles", (string)null);
                });

            modelBuilder.Entity("Core.Data.Entities.Comment", b =>
                {
                    b.HasOne("Core.Data.Entities.Comment", "ParentComment")
                        .WithMany("CommentReplies")
                        .HasForeignKey("ParentCommentId");

                    b.HasOne("Core.Data.Entities.Post", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Data.Entities.Profile", "UserProfile")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("ParentComment");

                    b.Navigation("Post");

                    b.Navigation("UserProfile");
                });

            modelBuilder.Entity("Core.Data.Entities.CommentReaction", b =>
                {
                    b.HasOne("Core.Data.Entities.Comment", "Comment")
                        .WithMany("Reactions")
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Comment");
                });

            modelBuilder.Entity("Core.Data.Entities.FileUrl", b =>
                {
                    b.HasOne("Core.Data.Entities.Post", "Post")
                        .WithMany("FileUrls")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");
                });

            modelBuilder.Entity("Core.Data.Entities.Follow", b =>
                {
                    b.HasOne("Core.Data.Entities.Profile", "Followed")
                        .WithMany("Followers")
                        .HasForeignKey("FollowedId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Core.Data.Entities.Profile", "Follower")
                        .WithMany("Following")
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Followed");

                    b.Navigation("Follower");
                });

            modelBuilder.Entity("Core.Data.Entities.FriendRequest", b =>
                {
                    b.HasOne("Core.Data.Entities.Profile", "ReceiverUser")
                        .WithMany("FriendRequestsAsReceiver")
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Core.Data.Entities.Profile", "SenderUser")
                        .WithMany("FriendRequestsAsSender")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("ReceiverUser");

                    b.Navigation("SenderUser");
                });

            modelBuilder.Entity("Core.Data.Entities.Friendship", b =>
                {
                    b.HasOne("Core.Data.Entities.Profile", "ReceiverUser")
                        .WithMany("FriendsAsReceiver")
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Core.Data.Entities.Profile", "SenderUser")
                        .WithMany("FriendsAsSender")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("ReceiverUser");

                    b.Navigation("SenderUser");
                });

            modelBuilder.Entity("Core.Data.Entities.Post", b =>
                {
                    b.HasOne("Core.Data.Entities.Profile", "UserProfile")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserProfile");
                });

            modelBuilder.Entity("Core.Data.Entities.PostReaction", b =>
                {
                    b.HasOne("Core.Data.Entities.Post", "Post")
                        .WithMany("Reactions")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");
                });

            modelBuilder.Entity("Core.Data.Entities.Comment", b =>
                {
                    b.Navigation("CommentReplies");

                    b.Navigation("Reactions");
                });

            modelBuilder.Entity("Core.Data.Entities.Post", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("FileUrls");

                    b.Navigation("Reactions");
                });

            modelBuilder.Entity("Core.Data.Entities.Profile", b =>
                {
                    b.Navigation("Followers");

                    b.Navigation("Following");

                    b.Navigation("FriendRequestsAsReceiver");

                    b.Navigation("FriendRequestsAsSender");

                    b.Navigation("FriendsAsReceiver");

                    b.Navigation("FriendsAsSender");
                });
#pragma warning restore 612, 618
        }
    }
}
