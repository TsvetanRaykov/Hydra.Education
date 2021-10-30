namespace Hydra.Server.Auth.Authorization
{
    public static class GlobalConstants
    {
        public static class Role
        {
            public const string AdministratorRoleName = "Admin";
            public const string StudentRoleName = "Student";
            public const string TrainerRoleName = "Trainer";
        }
        public static class SystemEmail
        {
            public const string SendFromEmail = "noreply@hydra-project.com";
            public const string SendFromName = "Your Hydra space";
        }
    }
}