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


        // GET: /api/profiles/

        [HttpGet]
        public async Task<IActionResult> GetProfiles()
        {
            List<ProfileResponse> profileResponses = await _profileService.GetProfilesAsync();

            return Ok(profileResponses);
        }


        // GET: /api/profiles/31faddd4-c910-45c2-a68b-bf67b5abaa77/

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetProfileById(Guid userId)
        {
            ProfileResponse profileResponse = await _profileService.GetProfileByIdAsync(userId);

            return Ok(profileResponse);
        }


        // PUT: /api/profiles/update-profile/

        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDto updateProfileDto)
        {
            Guid _userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value.ToString());

            ProfileResponse profileResponse = await _profileService.UpdateProfileAsync(updateProfileDto, _userId);

            return Ok(profileResponse);
        }
    }
}
