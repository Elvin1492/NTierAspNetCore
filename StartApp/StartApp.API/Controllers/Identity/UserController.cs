using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StartApp.API.ViewModels;
using StartApp.Core.Domain.Identity;
using StartApp.Service.Identity;

namespace StartApp.API.Controllers.Identity
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IAppUserService _appUserService;
        private readonly IAppRoleService _appRoleService;
        private readonly ITokenService _tokenService;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public UserController(IAppUserService appUserService, IAppRoleService appRoleService, ITokenService tokenService)
        {
            _appUserService = appUserService;
            _appRoleService = appRoleService;
            _tokenService = tokenService;
            _jsonSerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UserViewModel model)
        {
            if (model == null) return new StatusCodeResult(500);

            var user = await _appUserService.FindByNameAsync(model.UserName);

            if (user != null) return BadRequest("User name already exists");

            user = await _appUserService.FindByEmailAsync(model.Email);

            if (user != null) return BadRequest("Email already exists");

            var now = DateTime.Now;

            user = new AppUser()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                Email = model.Email,
                CreatedDate = now,
                LastModifiedDate = now,
                EmailConfirmed = true,
                LockoutEnabled = false
            };

            //string role_Administrator = "Administrator";
            //string role_RegisteredUser = "RegisteredUser";

            ////Create Roles (if they doesn't exist yet)

            //await _appRoleService.CreateAsync(new AppRole(role_Administrator));

            //await _appRoleService.CreateAsync(new AppRole(role_RegisteredUser));
            try
            {
                await _appUserService.CreateAsync(user, model.Password);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            //await _appUserService.AddToRoleAsync(user, "RegisteredUser");

            return Json(user.Adapt<UserViewModel>(), _jsonSerializerSettings);
        }

        [Authorize]
        [HttpPost("[action]")]
        public async Task<IActionResult> SignOut([FromBody] TokenRequestViewModel viewModel)
        {
            await HttpContext.SignOutAsync();
            _tokenService.RemoveByRefreshToken(viewModel.refresh_token);
            return Ok();
        }

        [Authorize]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _appUserService.FindByIdAsync(currentUserId);

            return Json(result);
        }

    }
}