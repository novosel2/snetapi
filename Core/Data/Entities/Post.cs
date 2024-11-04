using Core.Data.Dto.PostDto;
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

        public PostResponse ToPostResponse(bool includeProfile = true)
        {
            if (!includeProfile)
            {
                return new PostResponse()
                {
                    Id = Id,
                    Content = Content
                };
            }
            else
            {
                return new PostResponse()
                {
                    Id = Id,
                    Content = Content,
                    CreatedOn = CreatedOn,
                    UserProfile = UserProfile!.ToProfileResponse()
                };
            }
        }
    }
}
