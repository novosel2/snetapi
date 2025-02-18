﻿using Core.Data.Dto.ProfileDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Dto.FriendsDto
{
    public class FriendRequestResponse
    {
        public Guid FriendRequestId { get; set; }

        public ProfileResponse User { get; set; } = new ProfileResponse();

        public DateTime CreatedOn { get; set; }
    }
}
