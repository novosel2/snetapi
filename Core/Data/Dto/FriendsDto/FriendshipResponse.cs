using Core.Data.Dto.ProfileDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Dto.FriendsDto
{
    public class FriendshipResponse
    {
        public Guid FriendshipId { get; set; }

        public ProfileResponse User { get; set; } = new ProfileResponse();

        public DateTime CreatedOn { get; set; }
    }
}
