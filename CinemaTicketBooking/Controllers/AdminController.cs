// AdminController.cs
using CinemaTicketBooking.Data;
using Microsoft.AspNetCore.Mvc;

public class AdminController : Controller
{
    private readonly CinemaDbContext _dbContext;

    public AdminController(CinemaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Action to display users
    public IActionResult ShowUsers()
    {
        var users = _dbContext.Users.ToList();
        return View(users);
    }

    // Action to delete a user
    [HttpPost]
    public IActionResult DeleteUser(int userId)
    {
        var user = _dbContext.Users.Find(userId);

        if (user != null)
        {
            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
        }

        return RedirectToAction("ShowUsers");
    }
}
