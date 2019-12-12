using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using StartApp.Core.Domain.Identity;
using StartApp.Repository.Identity;

namespace StartApp.Service.Identity
{
    public interface IAppRoleService
    {
        List<AppRole> GetAll();
        Task<AppRole> FindByIdAsync(string roleId);
        Task<AppRole> FindByNameAsync(string roleName);
        Task<IList<Claim>> GetClaimsAsync(string roleId);
        Task<string> GetRoleIdAsync(AppRole role);
        Task<IdentityResult> UpdateAsync(AppRole role);
        Task<IdentityResult> CreateAsync(AppRole role);
        Task<IdentityResult> AddClaimAsync(string roleId, string claimValue);
        Task<IdentityResult> RemoveClaimAsync(string roleId, string claimValue);
        //Task<bool> UpdateClaimAsync(string oldClaimValue, string newClaimValue);
        Task<IdentityResult> DeleteByIdAsync(string roleId);
    }

    public class AppRoleService : IAppRoleService
    {
        private readonly IAppRoleRepository _AppRoleRepository;

        public AppRoleService(IAppRoleRepository AppRoleRepository)
        {
            this._AppRoleRepository = AppRoleRepository;
        }

        public List<AppRole> GetAll()
        {
            return _AppRoleRepository
            .GetAll()
            .OrderBy(role => role.Name)
            .ToList();
        }

        public async Task<AppRole> FindByIdAsync(string roleId)
        {
            return await _AppRoleRepository.FindByIdAsync(roleId);
        }

        public async Task<AppRole> FindByNameAsync(string roleName)
        {
            return await _AppRoleRepository.FindByNameAsync(roleName);
        }

        public async Task<IList<Claim>> GetClaimsAsync(string roleId)
        {
            var role = await _AppRoleRepository.FindByIdAsync(roleId);
            return await _AppRoleRepository.GetClaimsAsync(role);
        }

        public async Task<string> GetRoleIdAsync(AppRole role)
        {
            return await _AppRoleRepository.GetRoleIdAsync(role);
        }

        public async Task<IdentityResult> UpdateAsync(AppRole role)
        {
            return await _AppRoleRepository.UpdateAsync(role);
        }

        public async Task<IdentityResult> CreateAsync(AppRole role)
        {
            if (string.IsNullOrEmpty(role.Name) || string.IsNullOrWhiteSpace(role.Name))
            {
                throw new Exception("Role name is empty");
            }
            return await _AppRoleRepository.CreateAsync(role);
        }

        public async Task<IdentityResult> AddClaimAsync(string roleId, string claimValue)
        {
            if (string.IsNullOrEmpty(claimValue) || string.IsNullOrEmpty(claimValue))
            {
                throw new Exception("ClaimValue is empty");
            }

            var claim = new Claim(ClaimsIdentity.DefaultIssuer, claimValue);
            var role = await _AppRoleRepository.FindByIdAsync(roleId);

            return await _AppRoleRepository.AddClaimAsync(role, claim);
        }

        public async Task<IdentityResult> RemoveClaimAsync(string roleId, string claimValue)
        {
            var claim = new Claim(ClaimsIdentity.DefaultIssuer, claimValue);
            var role = await _AppRoleRepository.FindByIdAsync(roleId);

            return await _AppRoleRepository.RemoveClaimAsync(role, claim);
        }

        //public async Task<bool> UpdateClaimAsync(string oldClaimValue, string newClaimValue)
        //{
        //    var result = await _AppRoleRepository.UpdateClaim(oldClaimValue, newClaimValue);
        //    return result == 1;
        //}

        public async Task<IdentityResult> DeleteByIdAsync(string roleId)
        {
            return await _AppRoleRepository.DeleteByIdAsync(roleId);
        }
    }

    
}
