using System.ComponentModel.DataAnnotations;

namespace SocialMedia.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm Password is required")]
        public string ConfirmPassword { get; set; }
    }
}
