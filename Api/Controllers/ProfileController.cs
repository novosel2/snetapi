using Core.Data.Dto.ProfileDto;
using Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/profiles/")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        private readonly IBlobStorageService _blobStorageService;

        public ProfileController(IProfileService profileService, IBlobStorageService blobStorageService)
        {
            _profileService = profileService;
            _blobStorageService = blobStorageService;
        }


        // GET: /api/profiles

        [HttpGet]
        public async Task<IActionResult> GetProfiles()
        {
            List<ProfileResponse> profileResponses = await _profileService.GetProfilesAsync();

            return Ok(profileResponses);
        }


        // GET: /api/profiles/popular
        [HttpGet("popular")]
        public async Task<IActionResult> GetMostPopular(int limit = 10)
        {
            List<ProfileResponse> profileResponses = await _profileService.GetPopularAsync(limit);

            return Ok(profileResponses);
        }


        // GET: /api/profiles/31faddd4-c910-45c2-a68b-bf67b5abaa77

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetProfileById(Guid userId)
        {
            ProfileResponse profileResponse = await _profileService.GetProfileByIdAsync(userId);

            return Ok(profileResponse);
        }


        // GET: /api/profiles/friendship-status/31faddd4-c910-45c2-a68b-bf67b5abaa77

        [HttpGet("friendship-status/{userId}")]
        public async Task<IActionResult> GetFriendshipStatus(Guid userId)
        {
            ProfileFriendshipStatusDto friendshipStatus = await _profileService.GetProfileFriendshipStatusAsync(userId);

            return Ok(friendshipStatus);
        }


        // PUT: /api/profiles/update-profile

        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDto updateProfileDto)
        {
            ProfileResponse profileResponse = await _profileService.UpdateProfileAsync(updateProfileDto);

            return Ok(profileResponse);
        }


        // PUT: /api/profiles/update-profile-picture

        [HttpPut("update-profile-picture")]
        public async Task<IActionResult> UpdateProfilePicture(IFormFile image)
        {
            ProfileResponse profileResponse = await _profileService.UpdateProfilePictureAsync(image);

            return Ok(profileResponse);
        }


        // DELETE: /api/profiles/delete-profile-picture

        [HttpDelete("delete-profile-picture")]
        public async Task<IActionResult> DeleteProfilePicture()
        {
            ProfileResponse profileResponse = await _profileService.DeleteProfilePictureAsync();

            return Ok(profileResponse);
        }
    }
}
