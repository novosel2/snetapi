using Core.Data.Dto.PostDto;
using Core.Data.Dto.ProfileDto;
using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Dto.CommentDto
{
    public class CommentResponse
    {
        public Guid CommentId { get; set; }

        public string Content { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; }

        public ProfileInformationDto? User { get; set; }

        public int Likes { get; set; }
        public int Dislikes { get; set; }

        public ReactionType UserReacted { get; set; } = ReactionType.NoReaction;

        public List<CommentReplyDto> Replies { get; set; } = [];
    }
}
