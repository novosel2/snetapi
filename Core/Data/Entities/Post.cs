using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Entities
{
    public class Post
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        [ForeignKey(nameof(UserProfile))]
        public Guid ProfileId { get; set; }

        public Profile UserProfile { get; set; } = new Profile();
    }
}
