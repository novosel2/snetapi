using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface IBlobStorageService
    {
        /// <summary>
        /// Overwrites or creates profile picture in azure blob storage
        /// </summary>
        /// <param name="userId">Id of user you are adding picture to</param>
        /// <param name="image">Profile picture</param>
        /// <returns>Public Sas for the profile picture in blob storage</returns>
        public Task<string> UpdateProfilePictureByUserId(Guid userId, IFormFile image);

        /// <summary>
        /// Deletes profile picture
        /// </summary>
        /// <param name="pictureUrl">Url of the picture you want to delete</param>
        /// <returns>Default profile picture Sas</returns>
        public Task<string> DeleteProfilePictureByUrl(string pictureUrl);
    }
}
