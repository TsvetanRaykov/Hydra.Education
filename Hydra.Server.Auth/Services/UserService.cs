﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hydra.Server.Auth.Services
{
    using Contracts;
    using Models;

    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Dictionary<string, string> _roles;
        public UserService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roles = roleManager.Roles.ToDictionary(r => r.Id, r => r.Name);
        }

        public async Task<ApplicationUser[]> GetUserByRolesAsync(string[] roleIds)
        {
            var users = await _userManager.Users
                .Include(u => u.Claims)
                .Include(u => u.Roles)
                .ToArrayAsync();

            if (!roleIds.Any())
            {
                return users;
            }

            var query = users.Where(u => u.Roles.Select(r => r.RoleId).Intersect(roleIds).Any());

            return query.ToArray();
        }

        public Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return _userManager.Users
                .Include(u => u.Claims)
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<string> CreateUserAsync(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            var error = ProcessIdentityResult(result);

            if (string.IsNullOrWhiteSpace(error) && !string.IsNullOrWhiteSpace(user.FullName))
            {
                result = await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Name, user.FullName));
                return ProcessIdentityResult(result);
            }

            return null;
        }

        public async Task<string> UpdateUserAsync(ApplicationUser user, bool lockedOut)
        {
            var oldUser = await _userManager.FindByIdAsync(user.Id);

            if (oldUser == null) return "User not found.";

            oldUser.Email = user.Email;
            oldUser.LockoutEnd = lockedOut ? DateTimeOffset.MaxValue : default(DateTimeOffset?);

            var result = await _userManager.UpdateAsync(oldUser);

            if (!result.Succeeded) return ProcessIdentityResult(result);

            // update name
            var userClaims = await _userManager.GetClaimsAsync(oldUser);

            var nameClaim = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

            if (nameClaim == null || nameClaim.Value == user.FullName) return ProcessIdentityResult(result);

            await _userManager.RemoveClaimAsync(oldUser, nameClaim);
            await _userManager.AddClaimAsync(oldUser, new Claim(ClaimTypes.Name, user.FullName));

            return ProcessIdentityResult(result);
        }

        public async Task<string> SetPasswordAsync(ApplicationUser user, string password)
        {
            if (await _userManager.HasPasswordAsync(user))
            {
                await _userManager.RemovePasswordAsync(user);
            }

            var result = await _userManager.AddPasswordAsync(user, password);
            return ProcessIdentityResult(result);
        }

        public async Task<string> AddClaimAsync(ApplicationUser user, Claim claim)
        {
            var result = await _userManager.AddClaimAsync(user, claim);
            return ProcessIdentityResult(result);
        }

        public async Task<string> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return "User not found.";
            }

            var userClaims = await _userManager.GetClaimsAsync(user);

            foreach (var claim in userClaims)
            {
                await _userManager.RemoveClaimAsync(user, claim);
            }

            var roles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, roles);

            var result = await _userManager.DeleteAsync(user);
            return ProcessIdentityResult(result);
        }

        public async Task<string> AddToRoleAsync(ApplicationUser user, string roleName)
        {
            var result = await _userManager.AddToRoleAsync(user, roleName);
            return ProcessIdentityResult(result);
        }

        private string ProcessIdentityResult(IdentityResult result)
        {
            if (result.Succeeded)
            {
                return string.Empty;
            }

            var error = result.Errors.First().Description;
            return error;
        }
    }
}