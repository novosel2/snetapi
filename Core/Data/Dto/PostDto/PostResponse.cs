﻿using Core.Data.Dto.CommentDto;
using Core.Data.Dto.ProfileDto;
using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Dto.PostDto
{
    public class PostResponse
    {
        public Guid PostId { get; set; }

        public string Content { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; }

        public ProfileInformationDto? User { get; set; }

        public int Likes { get; set; } = 0;
        public int Dislikes { get; set; } = 0;

        public ReactionType UserReacted { get; set; } = ReactionType.NoReaction;

        public int CommentCount { get; set; } = 0;

        public List<string> FileUrls { get; set; } = [];
    }
}
