using System.ComponentModel.DataAnnotations;

namespace Hydra.Server.Auth.Pages.Auth.Models
{
    public class Passwords
    {
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, ErrorMessage = "Must be at least 6 characters.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        public void Clear()
        {
            Password = null;
            ConfirmPassword = null;
        }
    }
}