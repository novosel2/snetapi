using Core.Data.Dto.CommentDto;
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

        [Required]
        public Guid UserId { get; set; }
        public Profile? Profile { get; set; }

        public List<CommentReaction> Reactions { get; set; } = [];

        public Comment()
        {
            var cetZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            CreatedOn = TimeZoneInfo.ConvertTimeFromUtc(CreatedOn, cetZone);
        }

        public CommentResponse ToCommentResponse()
        {
            return new CommentResponse()
            {

            };
        }
    }
}
