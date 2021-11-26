﻿namespace Hydra.Server.Auth.Controllers
{
    using Authorization;
    using IdentityModel;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    //[tr]: 2021-10-26
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Identity.Application")]
        [AllowAnonymous]
        public IActionResult GetCurrentUser() =>
            Ok(User.Identity is { IsAuthenticated: true } ? CreateUserInfo(User) : UserInfo.Anonymous);

        private static UserInfo CreateUserInfo(ClaimsPrincipal claimsPrincipal)
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

                userInfo.Claims = claims;
            }

            return userInfo;
        }
    }
}
