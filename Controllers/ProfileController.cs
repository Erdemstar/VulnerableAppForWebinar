using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VulnerableAppForWebinar.Dto.Others;
using VulnerableAppForWebinar.Dto.Profile;
using VulnerableAppForWebinar.Entity.Profile;
using VulnerableAppForWebinar.Repository.Profile;
using VulnerableAppForWebinar.Utility.JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VulnerableAppForWebinar.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly ProfileRepository _profileRepository;
        private readonly JWTAuthManager _jwtAuthManager;

        public ProfileController(ProfileRepository profileRepository, JWTAuthManager jwtAuthManager)
        {
            _profileRepository = profileRepository;
            _jwtAuthManager = jwtAuthManager;
        }

        [HttpGet]
        [Authorize(Roles ="Admin,User")]
        public async Task<ActionResult> GetProfile([FromQuery] ProfileSearchRequest request)
        {
            var profile = await _profileRepository.GetProfileByEmail(request.Email);
            return Ok(profile);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> CreateProfile([FromBody] ProfileRequest request)
        {
            var email = _jwtAuthManager.TakeEmailFromJWT(Request.Headers["Authorization"].ToString().Split(" ")[1]);

            var profile = await _profileRepository.GetProfileByEmail(email);
            if (profile is null)
            {
                var prof = await _profileRepository.CreateProfile(new ProfileEntity { Email = email, Address = request.Address,
                    Birthday = request.Birthday, Hobby = request.Hobby });

                return Ok(new ProfileResponse { Address = prof.Address, Birthday = prof.Birthday , Hobby = prof.Hobby});
            }

            return BadRequest(new ErrorResponse() { Message = "Profile is already created" });
        }

        [HttpPut]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> UpdateProfile([FromBody] ProfileRequest request)
        {
            var email = _jwtAuthManager.TakeEmailFromJWT(Request.Headers["Authorization"].ToString().Split(" ")[1]);
            var profile = await _profileRepository.GetProfileByEmail(email);
            if (!(profile is null))
            {
                await _profileRepository.UpdateProfile(profile.Id, new ProfileEntity
                {
                    Id = profile.Id,
                    Address = request.Address,
                    Birthday = request.Birthday,
                    Hobby = request.Hobby,
                    Email = email,
                });
                return Ok(request);
            }

            return BadRequest(new ErrorResponse() { Message = "Profile is not created yet. This process can be done after profile is created" });

        }

        [HttpDelete]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> DeleteProfile([FromQuery] ProfileSearchRequest request)
        {
            var profile = await _profileRepository.GetProfileByEmail(request.Email);
            if (!(profile is null))
            {
                await _profileRepository.DeleteProfile(profile.Id);
            }

            return BadRequest(new ErrorResponse() { Message = "Profile is not created yet. This process can be done after profile is created" });
        }

    }
}
