// UserLoginViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.ViewModels
{
    public class UserLoginViewModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "The Role field is required.")]
        public string? Role { get; set; }
    }
}
