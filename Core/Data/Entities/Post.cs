using Core.Data.Dto.PostDto;
using Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entities
{
    public class Post
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [Required]
        [ForeignKey(nameof(UserProfile))]
        public Guid UserId { get; set; }
        public Profile? UserProfile { get; set; }

        public List<Comment> Comments { get; set; } = [];
        public List<PostReaction> Reactions { get; set; } = [];

        public Post()
        {
            TimeZoneInfo cetZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");

            CreatedOn = TimeZoneInfo.ConvertTimeFromUtc(CreatedOn, cetZone);
        }

        public PostResponse ToPostResponse(Guid currentUserId, bool includeProfile = true)
        {
            var postResponse = new PostResponse()
            {
                Id = Id,
                Content = Content,
                CreatedOn = CreatedOn,
                Likes = Reactions.Count(r => r.Reaction == ReactionType.Like),
                Dislikes = Reactions.Count(r => r.Reaction == ReactionType.Dislike),
                UserReacted = Reactions.Any(r => r.UserId == currentUserId) ? Reactions.First(r => r.UserId == currentUserId).Reaction : ReactionType.NoReaction,
                Comments = Comments.Select(c => c.ToCommentResponse(currentUserId)).ToList()
            };

            if (includeProfile)
            {
                postResponse.UserProfile = UserProfile!.ToProfileResponse();
            }

            return postResponse;
        }
    }
}
