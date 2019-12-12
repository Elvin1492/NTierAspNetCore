using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using StartApp.Core.Domain.Identity;
using StartApp.Repository.Identity;

namespace StartApp.Service.Identity
{
    public interface IAppUserService
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

    public class AppUserService : IAppUserService
    {
        private readonly IAppUserRepository _appUserRepository;

        public AppUserService(IAppUserRepository appUserRepository)
        {
            _appUserRepository = appUserRepository;
        }

        public async Task<AppUser> FindByIdAsync(string userId)
        {
            return await _appUserRepository.FindByIdAsync(userId);
        }

        public async Task<AppUser> FindByNameAsync(string name)
        {
            return await _appUserRepository.FindByNameAsync(name);
        }

        public async Task<AppUser> FindByEmailAsync(string email)
        {
            return await _appUserRepository.FindByEmailAsync(email);
        }

        public async Task<IdentityResult> CreateAsync(AppUser user, string password)
        {
            return await _appUserRepository.CreateAsync(user, password);
        }

        public async Task<IdentityResult> AddToRoleAsync(AppUser user, string roleName)
        {
            return await _appUserRepository.AddToRoleAsync(user, roleName);
        }

        public async Task<AppUser> FindByLoginAsync(string infoLoginProvider, string infoProviderKey)
        {
            return await _appUserRepository.FindByLoginAsync(infoLoginProvider, infoProviderKey);
        }

        public async Task<IList<string>> GetRolesAsync(AppUser user)
        {
            return await _appUserRepository.GetRolesAsync(user);
        }

        public async Task<IdentityResult> AddLoginAsync(AppUser user, UserLoginInfo info)
        {
            return await _appUserRepository.AddLoginAsync(user, info);
        }

        public async Task<bool> CheckPasswordAsync(AppUser user, string password)
        {
            return await _appUserRepository.CheckPasswordAsync(user, password);
        }
    }
}
