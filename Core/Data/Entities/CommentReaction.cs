using Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Entities
{
    public class CommentReaction
    {
        [Required]
        public Guid CommentId { get; set; }
        public Comment? Comment { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public ReactionType Reaction { get; set; }
    }
}
