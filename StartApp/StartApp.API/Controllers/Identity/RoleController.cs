using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StartApp.API.ViewModels;
using StartApp.Core.Domain.Identity;
using StartApp.Service.Identity;

namespace StartApp.API.Controllers.Identity
{
    [Route("api/[controller]")]
    [Authorize]
    public class RoleController : Controller
    {
        private readonly IAppRoleService _appRoleService;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public RoleController(IAppRoleService appRoleService)
        {
            this._appRoleService = appRoleService;
            _jsonSerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        [Route("[action]")]
        [HttpGet]
        //[Permission("Administrator")]
        public JsonResult GetAll()
        {
            List<AppRole> result = _appRoleService.GetAll();
            return Json(result, _jsonSerializerSettings);
        }

        [Route("[action]")]
        [HttpGet]
        //[Permission("Administrator")]
        public async Task<JsonResult> GetClaimsByRoleId(string roleId)
        {
            var result = await _appRoleService.GetClaimsAsync(roleId);
            return Json(result, _jsonSerializerSettings);
        }


        [Route("[action]")]
        [HttpPost]
        //[Permission("Administrator")]
        public async Task<IdentityResult> AddClaim([FromBody] RoleClaimViewModel roleClaim)
        {
            var result = await _appRoleService.AddClaimAsync(roleClaim.RoleId, roleClaim.ClaimValue);
            return result;
        }

        [Route("[action]")]
        [HttpPost]
        //[Permission("Administrator")]
        public async Task<IdentityResult> RemoveClaim([FromBody] RoleClaimViewModel roleClaim)
        {
            var result = await _appRoleService.RemoveClaimAsync(roleClaim.RoleId, roleClaim.ClaimValue);
            return result;
        }

        //[Route("[action]")]
        //[HttpPut]
        ////[Permission("Administrator")]
        //public async Task<bool> UpdateClaim([FromBody] UpdateClaimViewModel claim)
        //{
        //    var result = await _appRoleService.UpdateClaimAsync(claim.OldValue, claim.NewValue);
        //    return result;
        //}

        [Route("[action]")]
        [HttpPost]
        //[Permission("Administrator")]
        public async Task<IdentityResult> Create([FromBody] AppRole role)
        {
            var result = await _appRoleService.CreateAsync(role);
            return result;
        }

        //[Route("[action]")]
        //[HttpDelete("{roleId}")]
        ////[Permission("Administrator")]
        //public async Task<IdentityResult> Delete(string roleId)
        //{
        //    var result = await _appRoleService.DeleteByIdAsync(roleId);
        //    return result;
        //}

        [Route("[action]")]
        [HttpPut]
        //[Permission("Administrator")]
        public async Task<IdentityResult> Update([FromBody]AppRole role)
        {
            var result = await _appRoleService.UpdateAsync(role);
            return result;
        }
    }
}