using Core.Data.Dto.FriendsDto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Entities
{
    public class Friendship
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
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;


        public FriendshipResponse ToFriendshipResponse(Guid userId)
        {
            var friendshipResponse = new FriendshipResponse()
            {
                FriendshipId = Id,
                CreatedOn = CreatedOn
            };

            if (userId == ReceiverId)
                friendshipResponse.User = SenderUser!.ToProfileResponse();

            if (userId == SenderId)
                friendshipResponse.User = ReceiverUser!.ToProfileResponse();

            return friendshipResponse;
        }
    }
}
