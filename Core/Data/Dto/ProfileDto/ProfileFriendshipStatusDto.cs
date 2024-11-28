using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Dto.ProfileDto
{
    public class ProfileFriendshipStatusDto
    {
        public Guid UserId { get; set; }
        public bool IsFollowed { get; set; } = false;
        public Status FriendshipStatus { get; set; } = Status.None;
    }
}
