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

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }


        // GET: /api/profiles

        [HttpGet]
        public async Task<OkObjectResult> GetProfiles()
        {
            List<ProfileInformationDto> profileResponses = await _profileService.GetProfilesAsync();

            return Ok(profileResponses);
        }


        // GET: /api/profiles/search?searchTerm=John+Doe&limit=6

        [HttpGet("search")]
        public async Task<OkObjectResult> SearchProfilesAsync(string searchTerm, int limit = 6)
        {
            List<ProfileInformationDto> profileResponses = await _profileService.SearchProfilesAsync(searchTerm, limit);

            return Ok(profileResponses);
        }


        // GET: /api/profiles/popular
        [HttpGet("popular")]
        public async Task<OkObjectResult> GetMostPopular(int limit = 10)
        {
            List<ProfileInformationDto> profileResponses = await _profileService.GetPopularAsync(limit);

            return Ok(profileResponses);
        }


        // GET: /api/profiles/follow-suggestions
        [HttpGet("follow-suggestions")]
        public async Task<OkObjectResult> GetFollowSuggestions(int limit = 4)
        {
            List<Guid> suggestedUserIds = await _profileService.GetFollowSuggestionsAsync(limit);
            List<ProfileInformationDto> profiles = await _profileService.GetProfilesBatchAsync(suggestedUserIds);

            return Ok(profiles);
        }


        // GET: /api/profiles/mutual/31faddd4-c910-45c2-a68b-bf67b5abaa77
        [HttpGet("mutual/{userId}")]
        public async Task<OkObjectResult> GetMutualFriends(Guid userId, int limit = 4)
        {
            List<Guid> mutualIds = await _profileService.GetMutualFriendsIdsAsync(userId, limit);
            List<ProfileInformationDto> mutualFriends = await _profileService.GetProfilesBatchAsync(mutualIds);

            return Ok(mutualFriends);
        }


        // GET: /api/profiles/31faddd4-c910-45c2-a68b-bf67b5abaa77

        [HttpGet("{userId}")]
        public async Task<OkObjectResult> GetProfileById(Guid userId)
        {
            ProfileResponse profileResponse = await _profileService.GetProfileByIdAsync(userId);

            return Ok(profileResponse);
        }


        // GET: /api/profiles/username/novosel2

        [HttpGet("username/{username}")]
        public async Task<OkObjectResult> GetProfileByUsername(string username)
        {
            ProfileResponse profileResponse = await _profileService.GetProfileByUsernameAsync(username);

            return Ok(profileResponse);
        }


        // GET: /api/profiles/friendship-status/31faddd4-c910-45c2-a68b-bf67b5abaa77

        [HttpGet("friendship-status/{userId}")]
        public async Task<OkObjectResult> GetFriendshipStatus(Guid userId)
        {
            ProfileFriendshipStatusDto friendshipStatus = await _profileService.GetProfileFriendshipStatusAsync(userId);

            return Ok(friendshipStatus);
        }


        // PUT: /api/profiles/update-profile

        [HttpPut("update-profile")]
        public async Task<OkObjectResult> UpdateProfile(UpdateProfileDto updateProfileDto)
        {
            ProfileInformationDto profileResponse = await _profileService.UpdateProfileAsync(updateProfileDto);

            return Ok(profileResponse);
        }


        // PUT: /api/profiles/update-profile-picture

        [HttpPut("update-profile-picture")]
        public async Task<OkObjectResult> UpdateProfilePicture(IFormFile image)
        {
            ProfileInformationDto profileResponse = await _profileService.UpdateProfilePictureAsync(image);

            return Ok(profileResponse);
        }


        // DELETE: /api/profiles/delete-profile-picture

        [HttpDelete("delete-profile-picture")]
        public async Task<OkObjectResult> DeleteProfilePicture()
        {
            ProfileInformationDto profileResponse = await _profileService.DeleteProfilePictureAsync();

            return Ok(profileResponse);
        }
    }
}
