namespace Hydra.Server.Auth.Models
{
    public class ApplicationUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //E.g. Faculty Number
        public string IdentityNumber { get; set; }
        
        public string Email { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}