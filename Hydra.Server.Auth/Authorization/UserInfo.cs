﻿using System.Collections.Generic;

namespace Hydra.Server.Auth.Authorization
{
    public class UserInfo
    {
        public static readonly UserInfo Anonymous = new UserInfo();

        public bool IsAuthenticated { get; set; }

        public string NameClaimType { get; set; }

        public string RoleClaimType { get; set; }

        public ICollection<ClaimValue> Claims { get; set; }
    }
}