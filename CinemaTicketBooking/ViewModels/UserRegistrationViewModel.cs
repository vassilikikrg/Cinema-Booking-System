// UserRegistrationViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.ViewModels
{
    public class UserRegistrationViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Role { get; set; } // Added role property
    }
}
