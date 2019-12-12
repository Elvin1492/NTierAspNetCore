using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using StartApp.Core.Domain.Identity;

namespace StartApp.Repository.Identity
{
    public interface IAppRoleRepository
    {
        List<AppRole> GetAll();
        Task<AppRole> FindByIdAsync(string roleId);
        Task<AppRole> FindByNameAsync(string roleName);
        Task<IList<Claim>> GetClaimsAsync(AppRole role);
        Task<string> GetRoleIdAsync(AppRole role);
        Task<IdentityResult> UpdateAsync(AppRole role);
        Task<IdentityResult> CreateAsync(AppRole role);
        Task<IdentityResult> AddClaimAsync(AppRole role, Claim claim);
        Task<IdentityResult> RemoveClaimAsync(AppRole role, Claim claim);
        //Task<int> UpdateClaim(string oldClaimValue, string newClaimValue);
        Task<IdentityResult> DeleteByIdAsync(string roleId);
    }

    public class AppRoleRepository : IAppRoleRepository
    {
        private readonly RoleManager<AppRole> _roleManager;

        public AppRoleRepository(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public List<AppRole> GetAll()
        {
            return _roleManager.Roles.ToList();
        }

        public Task<AppRole> FindByIdAsync(string roleId)
        {
            return _roleManager.FindByIdAsync(roleId);
        }

        public Task<AppRole> FindByNameAsync(string roleName)
        {
            return _roleManager.FindByNameAsync(roleName);
        }

        public Task<IList<Claim>> GetClaimsAsync(AppRole role)
        {
            return _roleManager.GetClaimsAsync(role);
        }

        public Task<string> GetRoleIdAsync(AppRole role)
        {
            return _roleManager.GetRoleIdAsync(role);
        }

        public Task<IdentityResult> UpdateAsync(AppRole role)
        {
            return _roleManager.UpdateAsync(role);
        }

        public Task<IdentityResult> CreateAsync(AppRole role)
        {
            return _roleManager.CreateAsync(role);
        }

        public Task<IdentityResult> AddClaimAsync(AppRole role, Claim claim)
        {
            return _roleManager.AddClaimAsync(role, claim);
        }

        public Task<IdentityResult> RemoveClaimAsync(AppRole role, Claim claim)
        {
            return _roleManager.RemoveClaimAsync(role, claim);
        }

        //public Task<int> UpdateClaim(string oldClaimValue, string newClaimValue)
        //{
        //    var oldClaim = _context.RoleClaims.FirstOrDefault(x => x.ClaimValue == oldClaimValue);
        //    oldClaim.ClaimValue = newClaimValue;

        //    return _context.SaveChangesAsync();
        //}

        public async Task<IdentityResult> DeleteByIdAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            return await _roleManager.DeleteAsync(role);
        }
    }
}