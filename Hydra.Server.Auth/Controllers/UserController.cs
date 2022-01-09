namespace Hydra.Server.Auth.Controllers
{
    using Authorization;
    using Contracts;
    using IdentityModel;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    //[tr]: 2021-10-26
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IUserService _userService;
        public UserController(UserManager<ApplicationUser> userManager, IUserService userService, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _userService = userService;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Identity.Application")]
        [AllowAnonymous]
        public IActionResult GetCurrentUser() =>
            Ok(User.Identity is { IsAuthenticated: true } ? CreateUserInfo(User) : UserInfo.Anonymous);

        private UserInfo CreateUserInfo(ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal.Identity is { IsAuthenticated: false })
            {
                return UserInfo.Anonymous;
            }

            var userInfo = new UserInfo
            {
                IsAuthenticated = true
            };

            if (claimsPrincipal.Identity is ClaimsIdentity claimsIdentity)
            {
                userInfo.NameClaimType = claimsIdentity.NameClaimType;
                userInfo.RoleClaimType = claimsIdentity.RoleClaimType;
            }
            else
            {
                userInfo.NameClaimType = JwtClaimTypes.Name;
                userInfo.RoleClaimType = JwtClaimTypes.Role;
            }

            if (claimsPrincipal.Claims.Any())
            {
                var claims = new List<ClaimValue>();
                var nameClaims = claimsPrincipal.FindAll(userInfo.NameClaimType);
                var enumerable = nameClaims as Claim[] ?? nameClaims.ToArray();

                foreach (var claim in enumerable)
                {
                    claims.Add(new ClaimValue(userInfo.NameClaimType, claim.Value));
                }

                // To send additional claims to the client.
                foreach (var claim in claimsPrincipal.Claims.Except(enumerable))
                {
                    claims.Add(new ClaimValue(claim.Type, claim.Value));
                }

                var appUser = _userManager.GetUserAsync(User).GetAwaiter().GetResult();

                claims.Add(new ClaimValue(JwtClaimTypes.Id, appUser.IdentityNumber));
                claims.Add(new ClaimValue(ClaimTypes.GivenName, appUser.FullName));

                userInfo.Claims = claims;
            }

            return userInfo;
        }

        [HttpGet("students")]
        [Authorize(Roles = "Admin, Trainer")]
        public async Task<ActionResult<StudentDto[]>> GetStudents()
        {
            var roleStudent = await _roleManager.FindByNameAsync(GlobalConstants.Role.StudentRoleName);
            var appStudents = await _userService.GetUserByRolesAsync(new[] { roleStudent.Id });

            return appStudents.Select(s => new StudentDto
            {
                IdentityNumber = s.IdentityNumber,
                FullName = s.FullName,
                UserName = s.UserName
            })
                .ToArray();
        }
    }
}
