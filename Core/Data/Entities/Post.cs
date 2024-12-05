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
        public DateTime CreatedOn { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public Profile? User { get; set; }

        public int CommentCount { get; set; } = 0;
        public List<Comment> Comments { get; set; } = [];
        public List<PostReaction> Reactions { get; set; } = [];

        public List<FileUrl> FileUrls { get; set; } = [];

        public Post()
        {
            TimeZoneInfo cetZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");

            CreatedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cetZone);
        }

        public PostResponse ToPostResponse(Guid currentUserId)
        {
            var postResponse = new PostResponse()
            {
                PostId = Id,
                Content = Content,
                CreatedOn = CreatedOn,
                Likes = Reactions.Count(r => r.Reaction == ReactionType.Like),
                Dislikes = Reactions.Count(r => r.Reaction == ReactionType.Dislike),
                UserReacted = Reactions.Any(r => r.UserId == currentUserId) ? Reactions.First(r => r.UserId == currentUserId).Reaction : ReactionType.NoReaction,
                CommentCount = CommentCount,
                User = User!.ToProfileInformation(),
                FileUrls = FileUrls.Select(fu => fu.Url).ToList()
            };

            return postResponse;
        }
    }
}
