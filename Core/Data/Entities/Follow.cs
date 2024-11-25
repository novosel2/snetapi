using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Entities
{
    public class Follow
    {
        [Key]
        public Guid? Id { get; set; }

        public Guid? FollowerId { get; set; }
        public Profile? Follower { get; set; }

        public Guid? FollowedId { get; set; }
        public Profile? Followed { get; set; }
    }
}
