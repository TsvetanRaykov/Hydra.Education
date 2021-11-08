using System.Security.Claims;
using System.Threading.Tasks;
using Hydra.Server.Auth.Models;

namespace Hydra.Server.Auth.Contracts
{
    public interface IUserService
    {
        Task<ApplicationUser[]> GetUserByRolesAsync(string[] roleIds);
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        Task<string> CreateUserAsync(ApplicationUser user, string password);
        Task<string> UpdateUserAsync(ApplicationUser user);
        Task<string> SetPasswordAsync(ApplicationUser user, string password);
        Task<string> AddClaimAsync(ApplicationUser user, Claim claim);
        Task<string> DeleteUserAsync(string userId);
        Task<string> AddToRoleAsync(ApplicationUser user, string roleName);
    }
}