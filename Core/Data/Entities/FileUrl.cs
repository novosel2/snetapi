using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Entities
{
    public class FileUrl
    {
        [Required]
        public Guid PostId { get; set; }
        public Post? Post { get; set; }

        [Required]
        public string Url { get; set; } = string.Empty;
    }
}
