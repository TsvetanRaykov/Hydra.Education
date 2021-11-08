using Hydra.Server.Auth.Models;
using Hydra.Server.Auth.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hydra.Server.Auth.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public Task<ApplicationUser[]> GetUserByRolesAsync(string[] roleIds)
        {
            //var users = await _userManager.Users
            //    .Include(u => u.Claims)
            //    .Include(u => u.Roles)
            //    .ToArrayAsync();

            //if (!roleIds.Any())
            //{
            //    return users;
            //}

            //var query = users.Where(u => u.Roles.Select(r => r.RoleId).Intersect(roleIds).Any());

            //return query.ToArray();
            return Task.FromResult(Array.Empty<ApplicationUser>());
        }

        public Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> CreateUserAsync(ApplicationUser user, string password)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> UpdateUserAsync(ApplicationUser user)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> SetPasswordAsync(ApplicationUser user, string password)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> AddClaimAsync(ApplicationUser user, Claim claim)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> DeleteUserAsync(string userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> AddToRoleAsync(ApplicationUser user, string roleName)
        {
            throw new System.NotImplementedException();
        }
    }
}