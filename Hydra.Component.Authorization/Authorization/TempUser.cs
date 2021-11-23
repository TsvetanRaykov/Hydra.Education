using System.Collections.Generic;

namespace Hydra.Component.Authorization.Authorization
{
    public class TempUser
    {
        public string Name { get; init; }
        public List<string> Roles { get; init; } = new();
    }
}