using System.Collections.Generic;

namespace Hydra.Module.Video.Backend.Authentication
{
    public static class Data
    {
        // TODO: Move it to user secrets
        public static Dictionary<string, string> Clients = new()
        {
            { "VideoModuleClient", "d3d2985a-9726-4bf4-8dfc-fa67948bc16f" }
        };
    }
}