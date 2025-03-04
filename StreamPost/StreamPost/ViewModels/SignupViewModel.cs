using System.ComponentModel.DataAnnotations;

namespace StreamPost.ViewModels
{
    public class SignupViewModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone number is required.")]
        [MaxLength(10, ErrorMessage = "Phone number cannot be longer than 10 digits.")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Gender is required.")]
        public char Gender { get; set; }
        [Required(ErrorMessage = "Date of birth is required.")]
        public string DateOfBirth { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm password is required.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
