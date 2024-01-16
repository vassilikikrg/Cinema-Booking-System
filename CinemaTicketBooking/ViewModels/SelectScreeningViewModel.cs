using CinemaTicketBooking.Models;

namespace CinemaTicketBooking.ViewModels
{
    public class SelectScreeningViewModel
    {
        public Movie? Movie { get; set; }
        public List<Screening>? Screenings { get; set; }
    }
}
