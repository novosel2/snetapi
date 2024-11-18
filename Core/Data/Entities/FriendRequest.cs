using Core.Data.Dto.FriendsDto;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Entities
{
    public class FriendRequest
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid ReceiverId { get; set; }
        public Profile? ReceiverUser { get; set; }

        [Required]
        public Guid SenderId { get; set; }
        public Profile? SenderUser { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public FriendRequest()
        {
            TimeZoneInfo cetZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");

            CreatedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cetZone);
        }

        public FriendRequestResponse ToFriendRequestResponse(Guid currentUserId)
        {
            var friendRequestResponse = new FriendRequestResponse()
            {
                Id = Id
            };

            if (currentUserId == ReceiverId)
                friendRequestResponse.User = SenderUser!.ToProfileResponse();

            if (currentUserId == SenderId)
                friendRequestResponse.User = ReceiverUser!.ToProfileResponse();

            return friendRequestResponse;
        }
    }
}
