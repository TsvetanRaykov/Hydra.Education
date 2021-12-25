namespace Hydra.Component.Authorization
{
    public class Endpoints
    {
        public string SignIn { get; set; }
        public string SignOut { get; set; }
        public string User { get; set; }

        // The public list of users that API expose to the authorized clients
        public string Users { get; set; }
    }
}