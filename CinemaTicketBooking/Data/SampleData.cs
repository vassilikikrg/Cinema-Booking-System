using CinemaTicketBooking.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBooking.Data
{
    public class SampleData
    {
        public static void Initialize(CinemaDbContext context)
        {
            string[] roles = new string[] { "Customer", "Admin", "Content_Admin"};



            var user = new User
            {
                Username = "Test",
                Email = "test@test.com",
                Password = "123456",
                CreateTime = DateTime.Now,
                Salt="Test",
                Role = roles[2]
            };


            var cinema=new Cinema { Name = "Village Metro Mall 1", Seats=300, _3d="blah" };

            context.Users.Add(user);
            context.Cinemas.Add(cinema);
            context.SaveChanges();
            var content_admin = new ContentAdmin { UserId = user.Id, Name = "Test Name" };
            context.ContentAdmins.Add(content_admin);
            context.SaveChanges();
        }
    }
}
