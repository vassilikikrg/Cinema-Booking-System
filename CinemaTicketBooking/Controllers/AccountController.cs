// AccountController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using CinemaTicketBooking.Models;
using CinemaTicketBooking.Data;
using CinemaTicketBooking.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace CinemaTicketBooking.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly CinemaDbContext _dbContext; // Replace with your actual DbContext

        public AccountController(ILogger<AccountController> logger, CinemaDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel userLogin)
        {
            // Validate the model
            if (!ModelState.IsValid)
            {
                return View(userLogin);
            }

            // Basic authentication logic (replace with secure authentication mechanism)
            var existingUser = _dbContext.Users
                .SingleOrDefault(u => u.Username == userLogin.Username);

            if (existingUser == null || !IsPasswordValid(userLogin.Password, existingUser.Password, existingUser.Salt))
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View(userLogin);
            }

            var selectedRole = userLogin.Role;

            // Add the selected role to claims or store it in the session
            // This depends on how you handle roles and claims in your application
            // For simplicity, let's assume storing it in a claim for now
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userLogin.Username),
            new Claim(ClaimTypes.Role, selectedRole)
        };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);


            // Redirect to home page on successful login
            return RedirectToAction("Index", "Home");

           /* if (existingUser == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid username.");
                return View();
            }

            // Redirect to home page on successful login
            return RedirectToAction("Index", "Home");*/
        }

        private bool IsPasswordValid(string enteredPassword, string storedPassword, string salt)
        {
            // Implement your password validation logic here
            // Example: Hash the entered password with the stored salt and compare with the stored password
            string hashedPassword = HashPassword(enteredPassword, salt);
            return storedPassword == hashedPassword;
        }

        private string HashPassword(string password, string salt)
        {
            // Implement your password hashing logic here
            // Example: Use a secure hashing algorithm like bcrypt
            // In a real-world scenario, consider using a library like BCrypt.Net
            // For simplicity, a basic hash is shown here (not recommended for production)
            return password + salt;
        }

        public IActionResult Logout()
        {
            // Implement logout logic here
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogoutConfirmed()
        {
            // Implement logout confirmation logic (if needed)

            // Sign out the user
            await HttpContext.SignOutAsync();

            // Redirect to the home page or another page after logout
            return RedirectToAction("Index", "Home");
        }
    }
}
