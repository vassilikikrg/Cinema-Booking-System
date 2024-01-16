using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace CinemaTicketBooking.ViewModels
{
    public class AddScreeningViewModel
    {
        [DisplayName("Movie")]
        public int MovieId { get; set; }

        [DisplayName("Cinema Hall")]
        public int CinemaId { get; set; }

        [DisplayName("Date and Starting Time")]
        public DateTime StartDateAndTime { get; set; }
    }
}
