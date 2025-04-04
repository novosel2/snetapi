﻿using Core.Data.Dto.ProfileDto;
using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Dto.CommentDto
{
    public class CommentReplyDto
    {
        public Guid CommentReplyId { get; set; }

        public string Content { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; }

        public ProfileInformationDto? UserProfile { get; set; }

        public int Likes { get; set; }
        public int Dislikes { get; set; }

        public ReactionType UserReacted { get; set; } = ReactionType.NoReaction;
    }
}
