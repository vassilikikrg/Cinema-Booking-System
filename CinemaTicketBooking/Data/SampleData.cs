using System;
using System.Linq;
using CinemaTicketBooking.Cryptography;
using CinemaTicketBooking.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaTicketBooking.Data
{
    public static class SampleData
    {

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();

                PasswordHasher passwordHasher = new PasswordHasher();
                // Ensure the database is created
                context.Database.EnsureCreated();

                // Check if there are any users in the database
                if (!context.Users.Any())
                {
                    // Seed an admin user
                    var id = AddUser(context, "admin@test.com", "Admin", "123456", "Admin", passwordHasher);
                    context.Admins.Add(new Admin { Name = "Admin Name", UserId = id });
                    // Seed a content admin user
                    id = AddUser(context, "contentadmin@test.com", "ContentAdmin", "123456", "Content_Admin", passwordHasher);
                    context.ContentAdmins.Add(new ContentAdmin { Name = "ContentAdmin Name", UserId = id });

                    // Seed a customer user
                    id = AddUser(context, "customer@test.com", "Customer", "123456", "Customer", passwordHasher);
                    context.Customers.Add(new Customer { Name = "ContentAdmin Name", UserId = id });
                    context.SaveChanges();
                }
                if (!context.Cinemas.Any())
                {
                    for (int i = 0; i < 10; i++)
                    {
                        var cinema = new Cinema { Name = "Village Metro Mall " + i.ToString(), Seats = i * 20, _3d = "Yes" };
                        context.Cinemas.Add(cinema);
                    }
                }
            }
        }

        private static int AddUser(CinemaDbContext context, string email, string userName, string password, string role, PasswordHasher passwordHasher)
        {
            var salt = passwordHasher.GenerateSalt();
            var hashedpwd=passwordHasher.HashPassword(password, salt);
            var user = new User
            {
                Email = email,
                Username = userName,
                Password = hashedpwd,
                CreateTime = DateTime.Now,
                Salt =salt,
                Role = role
            };

            context.Add(user);
            context.SaveChanges();
            return user.Id;
        }
    }
}
