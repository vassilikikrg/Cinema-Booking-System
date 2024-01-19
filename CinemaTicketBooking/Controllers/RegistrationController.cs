// RegistrationController.cs (create a new controller for registration)
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using CinemaTicketBooking.Models;
using CinemaTicketBooking.ViewModels;
using CinemaTicketBooking.Data;
using CinemaTicketBooking.Cryptography;

namespace CinemaTicketBooking.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly ILogger<RegistrationController> _logger;
        private readonly CinemaDbContext _dbContext;
        private PasswordHasher _passwordHasher;

        public RegistrationController(ILogger<RegistrationController> logger, CinemaDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
            _passwordHasher = new PasswordHasher();
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

            // Generate a unique salt for each user 
            string salt = _passwordHasher.GenerateSalt();

            // Hash the password with the generated salt
            string hashedPassword = _passwordHasher.HashPassword(userRegister.Password, salt);
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
    }
}
