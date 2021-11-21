namespace Hydra.Server.Auth.Utilities
{
    using Models;

    public static class UserValidation
    {
        public static bool ValidateAccountCompletion(ApplicationUser user)
        {
            return !string.IsNullOrWhiteSpace(user.FullName) && !string.IsNullOrWhiteSpace(user.IdentityNumber);
        }
    }
}