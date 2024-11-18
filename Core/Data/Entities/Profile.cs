﻿using Core.Data.Dto.ProfileDto;
using Core.Data.Entities.Identity;
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
        public List<Friendship> FriendsAsReciever { get; set; } = [];

        public List<FriendRequest> FriendRequestsAsSender { get; set; } = [];
        public List<FriendRequest> FriendRequestsAsReciever { get; set; } = [];

        public ProfileResponse ToProfileResponse()
        {
            return new ProfileResponse()
            {
                Id = Id,
                Username = Username,
                FirstName = FirstName,
                LastName = LastName,
                PictureUrl = PictureUrl
            };
        }
    }
}
