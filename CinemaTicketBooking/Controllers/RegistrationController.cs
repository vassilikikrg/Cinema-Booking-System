// RegistrationController.cs (create a new controller for registration)
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using CinemaTicketBooking.Models;
using CinemaTicketBooking.ViewModels;
using CinemaTicketBooking.Data;

namespace CinemaTicketBooking.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly ILogger<RegistrationController> _logger;
        private readonly CinemaDbContext _dbContext; // Replace with your actual DbContext

        public RegistrationController(ILogger<RegistrationController> logger, CinemaDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserRegistrationViewModel userRegister)
        {
            // Validate the model
            if (!ModelState.IsValid)
            {
                return View(userRegister);
            }

            // Check if the username is already taken
            if (_dbContext.Users.Any(u => u.Username == userRegister.Username))
            {
                ModelState.AddModelError("Username", "Username is already taken.");
                return View(userRegister);
            }

            // Generate a unique salt for each user (you can use a more secure method)
            string salt = Guid.NewGuid().ToString("N");

            // Hash the password with the generated salt
            string hashedPassword = HashPassword(userRegister.Password, salt);

            // Save the user to the database
            var newUser = new User
            {
                Username = userRegister.Username,
                Email = userRegister.Email,
                Password = hashedPassword,
                Salt = salt,
                CreateTime = DateTime.Now,
                Role = userRegister.Role // Set the default role, modify as needed
            };

            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();

            // Redirect to login page after successful registration
            return RedirectToAction("Login", "Account");
        }

        private string HashPassword(string password, string salt)
        {
            // Implement your password hashing logic here
            // Example: Use a secure hashing algorithm like bcrypt
            // In a real-world scenario, consider using a library like BCrypt.Net
            // For simplicity, a basic hash is shown here (not recommended for production)
            return password + salt;
        }
    }
}
