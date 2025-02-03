﻿// <auto-generated />
using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

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
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Core.Data.Entities.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("ParentCommentId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ParentCommentId");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId", "PostId", "CreatedOn");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Core.Data.Entities.CommentReaction", b =>
                {
                    b.Property<Guid>("CommentId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<int>("Reaction")
                        .HasColumnType("integer");

                    b.HasKey("CommentId", "UserId");

                    b.ToTable("CommentReactions");
                });

            modelBuilder.Entity("Core.Data.Entities.FileUrl", b =>
                {
                    b.Property<Guid>("PostId")
                        .HasColumnType("uuid");

                    b.Property<string>("Url")
                        .HasColumnType("text");

                    b.HasKey("PostId", "Url");

                    b.HasIndex("PostId");

                    b.ToTable("FileUrls");
                });

            modelBuilder.Entity("Core.Data.Entities.Follow", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("FollowedId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("FollowerId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("FollowedId");

                    b.HasIndex("FollowerId", "FollowedId")
                        .IsUnique();

                    b.ToTable("Follows");
                });

            modelBuilder.Entity("Core.Data.Entities.FriendRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ReceiverId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SenderId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("SenderId", "ReceiverId")
                        .IsUnique();

                    b.ToTable("FriendRequests");
                });

            modelBuilder.Entity("Core.Data.Entities.Friendship", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ReceiverId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SenderId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("SenderId", "ReceiverId")
                        .IsUnique();

                    b.ToTable("Friendships");
                });

            modelBuilder.Entity("Core.Data.Entities.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("CommentCount")
                        .HasColumnType("integer");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double>("PopularityScore")
                        .HasColumnType("double precision");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CreatedOn");

                    b.HasIndex("Id");

                    b.HasIndex("PopularityScore");

                    b.HasIndex("UserId");

                    b.HasIndex("CreatedOn", "PopularityScore", "UserId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Core.Data.Entities.PostReaction", b =>
                {
                    b.Property<Guid>("PostId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Reaction")
                        .HasColumnType("integer");

                    b.HasKey("PostId", "UserId");

                    b.HasIndex("PostId", "CreatedOn");

                    b.HasIndex("PostId", "UserId");

                    b.ToTable("PostReactions");
                });

            modelBuilder.Entity("Core.Data.Entities.Profile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<int>("FollowersCount")
                        .HasColumnType("integer");

                    b.Property<int>("FollowingCount")
                        .HasColumnType("integer");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("Occupation")
                        .HasColumnType("text");

                    b.Property<string>("PictureUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PreviousFollowers")
                        .HasColumnType("integer");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("FirstName");

                    b.HasIndex("Id");

                    b.HasIndex("LastName");

                    b.HasIndex("Username");

                    b.HasIndex("Username", "FirstName", "LastName");

                    b.ToTable("Profiles");
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

                    b.HasOne("Core.Data.Entities.Profile", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("ParentComment");

                    b.Navigation("Post");

                    b.Navigation("User");
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
                    b.HasOne("Core.Data.Entities.Profile", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
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
