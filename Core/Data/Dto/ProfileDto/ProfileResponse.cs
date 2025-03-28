﻿using Core.Data.Dto.PostDto;
using Core.Data.Entities;
using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Dto.ProfileDto
{
    public class ProfileResponse
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Description { get; set; }
        public string? Occupation { get; set; }
        public string PictureUrl { get; set; } = string.Empty;
        public int Followers { get; set; }
        public int Following { get; set; }

        public Profile ToProfile()
        {
            return new Profile()
            {
                Id = UserId,
                Username = Username,
                FirstName = FirstName,
                LastName = LastName,
                PictureUrl = PictureUrl
            };
        }
    }
}
