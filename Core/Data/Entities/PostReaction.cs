using Core.Enums;
using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;

namespace Core.Data.Entities
{
    public class PostReaction
    {
        [Required]
        public Guid PostId { get; set; }
        public Post? Post { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public ReactionType Reaction { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
