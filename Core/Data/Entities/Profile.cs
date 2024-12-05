using Core.Data.Dto.ProfileDto;
using Core.Data.Entities.Identity;
using Core.Enums;
using Core.IServices;
using Core.Services;
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

        [Required]
        public string PictureUrl { get; set; } = "https://snetblobstorage.blob.core.windows.net/snetprofiles/default.jpg";

        public List<Friendship> FriendsAsSender { get; set; } = [];
        public List<Friendship> FriendsAsReceiver { get; set; } = [];

        public List<FriendRequest> FriendRequestsAsSender { get; set; } = [];
        public List<FriendRequest> FriendRequestsAsReceiver { get; set; } = [];

        public List<Follow> Followers { get; set; } = [];
        public List<Follow> Following { get; set; } = [];

        [Required]
        public int FollowersCount { get; set; } = 0;

        [Required]
        public int FollowingCount { get; set; } = 0;

        [Required]
        public int PreviousFollowers { get; set; } = 0;

        [NotMapped]
        public double PopularityScore { get; set; }

        public ProfileResponse ToProfileResponse()
        {
            return new ProfileResponse()
            {
                UserId = Id,
                Username = Username,
                FirstName = FirstName,
                LastName = LastName,
                PictureUrl = PictureUrl,
                Followers = FollowersCount,
                Following = FollowingCount
            };
        }

        public ProfileInformationDto ToProfileInformation()
        {
            return new ProfileInformationDto()
            {
                UserId = Id,
                Username = Username,
                FirstName = FirstName,
                LastName = LastName,
                PictureUrl = PictureUrl
            };
        }
    }
}
