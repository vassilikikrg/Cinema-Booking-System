using CinemaTicketBooking.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBooking.Data
{
    public class SampleData
    {
        public static void Initialize(CinemaDbContext context)
        {
            for(int i=0; i<10; i++)
            {
                var cinema = new Cinema { Name = "Village Metro Mall " + i.ToString(), Seats = i*20, _3d = "Yes" };
                context.Cinemas.Add(cinema);
            }

            context.SaveChanges();
        }
    }
}
