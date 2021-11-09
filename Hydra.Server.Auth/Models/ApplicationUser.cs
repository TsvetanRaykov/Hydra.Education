using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Hydra.Server.Auth.Models
{
    using Contracts;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }
        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }

        // Hydra specific
        public string IdentityNumber { get; set; }
        public string FullName { get; set; }
    }
}