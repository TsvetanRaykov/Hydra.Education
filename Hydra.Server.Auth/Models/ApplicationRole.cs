using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Hydra.Server.Auth.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() { }

        public ApplicationRole(string roleName)
            : base(roleName) { }

        public virtual ICollection<IdentityRoleClaim<string>> Claims { get; set; }
    }
}