namespace Hydra.Component.Authorization
{
    using System;

    public class Endpoints
    {
        public Uri BaseUrl { get; set; }
        public string SignIn { get; set; }
        public string SignOut { get; set; }
        public string User { get; set; }

        // The public list of users that API expose to the authorized clients
        public string Users { get; set; }
    }
}