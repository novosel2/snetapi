using Core.Data.Dto.CommentDto;
using Core.Data.Dto.ProfileDto;
using Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Entities
{
    public class Comment
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [Required]
        public Guid PostId { get; set; }
        public Post? Post { get; set; }

        public Guid? ParentCommentId { get; set; }
        public Comment? ParentComment { get; set; }

        [Required]
        public Guid UserId { get; set; }
        public Profile? User { get; set; }

        public List<CommentReaction> Reactions { get; set; } = [];

        public List<Comment> CommentReplies { get; set; } = [];


        public CommentResponse ToCommentResponse(Guid currentUserId)
        {
            return new CommentResponse()
            {
                CommentId = Id,
                Content = Content,
                CreatedOn = CreatedOn,
                User = User!.ToProfileInformation(),
                Likes = Reactions.Count(r => r.Reaction is ReactionType.Like),
                Dislikes = Reactions.Count(r => r.Reaction is ReactionType.Dislike),
                UserReacted = Reactions.Any(r => r.UserId == currentUserId) ? Reactions.First(r => r.UserId == currentUserId).Reaction : ReactionType.NoReaction,
                Replies = CommentReplies.Select(cr => cr.ToCommentReply(currentUserId)).OrderBy(c => c.CreatedOn).ToList()
            };
        }

        public CommentReplyDto ToCommentReply(Guid currentUserId)
        {
            return new CommentReplyDto()
            {
                CommentReplyId = Id,
                Content = Content,
                CreatedOn = CreatedOn,
                UserProfile = User!.ToProfileInformation(),
                Likes = Reactions.Count(r => r.Reaction is ReactionType.Like),
                Dislikes = Reactions.Count(r => r.Reaction is ReactionType.Dislike),
                UserReacted = Reactions.Any(r => r.UserId == currentUserId) ? Reactions.First(r => r.UserId == currentUserId).Reaction : ReactionType.NoReaction
            };
        }
    }
}
