using Core.Data.Dto.ProfileDto;
using Core.Data.Entities.Identity;
using Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.WebRequestMethods;

namespace Core.Data.Entities
{
    public class Profile
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string PictureUrl { get; set; } = "https://snetblobstorage.blob.core.windows.net/snetprofiles/default.jpg";

        public List<Friendship> FriendsAsSender { get; set; } = [];
        public List<Friendship> FriendsAsReceiver { get; set; } = [];

        public List<FriendRequest> FriendRequestsAsSender { get; set; } = [];
        public List<FriendRequest> FriendRequestsAsReceiver { get; set; } = [];

        public List<Follow> Followers { get; set; } = [];
        public List<Follow> Following { get; set; } = [];

        public ProfileResponse ToProfileResponse()
        {
            ProfileResponse profileResponse = new ProfileResponse()
            {
                Id = Id,
                Username = Username,
                FirstName = FirstName,
                LastName = LastName,
                PictureUrl = PictureUrl,
                Followers = Followers.Count,
                Following = Following.Count
            };

            //if (FriendsAsReceiver.Any(f => f.SenderId == currentUserId) || FriendsAsSender.Any(f => f.ReceiverId == currentUserId))
            //    profileResponse.CurrentUserStatus = Status.Friends;

            //else if (Followers.Any(f => f.FollowerId == currentUserId))
            //    profileResponse.CurrentUserStatus = Status.Following;

            //if (FriendRequestsAsSender.Any(fr => fr.ReceiverId == currentUserId))
            //    profileResponse.CurrentUserStatus = Status.ReceivedRequest;

            //if (FriendRequestsAsReceiver.Any(fr => fr.SenderId == currentUserId))
            //    profileResponse.CurrentUserStatus = Status.SentRequest;

            return profileResponse;
        }
    }
}
