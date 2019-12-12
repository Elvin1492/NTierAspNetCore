using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using StartApp.Core.Domain.Identity;

namespace StartApp.Repository.Identity
{
    public interface IAppUserRepository
    {
        Task<AppUser> FindByIdAsync(string userId);
        Task<AppUser> FindByNameAsync(string name);
        Task<AppUser> FindByEmailAsync(string email);
        Task<IdentityResult> CreateAsync(AppUser user, string password);
        Task<IdentityResult> AddToRoleAsync(AppUser user, string roleName);
        Task<AppUser> FindByLoginAsync(string infoLoginProvider, string infoProviderKey);
        Task<IList<string>> GetRolesAsync(AppUser user);
        Task<IdentityResult> AddLoginAsync(AppUser user, UserLoginInfo info);
        Task<bool> CheckPasswordAsync(AppUser user, string password);
    }

    public class AppUserRepository : IAppUserRepository
    {
        private readonly UserManager<AppUser> _userManager;

        public AppUserRepository(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AppUser> FindByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<AppUser> FindByNameAsync(string name)
        {
            return await _userManager.FindByNameAsync(name);
        }

        public async Task<AppUser> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<IdentityResult> CreateAsync(AppUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> AddToRoleAsync(AppUser user, string roleName)
        {
            return await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<AppUser> FindByLoginAsync(string infoLoginProvider, string infoProviderKey)
        {
            return await _userManager.FindByLoginAsync(infoLoginProvider, infoProviderKey);
        }

        public async Task<IList<string>> GetRolesAsync(AppUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<IdentityResult> AddLoginAsync(AppUser user, UserLoginInfo info)
        {
            return await _userManager.AddLoginAsync(user, info);
        }

        public async Task<bool> CheckPasswordAsync(AppUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }
    }
}