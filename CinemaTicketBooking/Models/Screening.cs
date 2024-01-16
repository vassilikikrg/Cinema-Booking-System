using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBooking.Models
{
    [Table("screenings")]
    public partial class Screening
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("movie_id")]
        [DisplayName("Movie")]
        public int MovieId { get; set; }

        [Column("cinema_id")]
        [DisplayName("Cinema Hall")]
        public int CinemaId { get; set; }

        [Column("start_date_time")]
        [DisplayName("Date and Starting Time")]
        public DateTime StartDateAndTime { get; set; }

        [Column("available_seats")]
        [DisplayName("Available Seats")]
        public int AvailableSeats { get; set; }

        [ForeignKey("CinemaId")]
        [InverseProperty("Screenings")]
        public virtual Cinema? Cinema { get; set; }

        [ForeignKey("MovieId")]
        [InverseProperty("Screenings")]
        public virtual Movie? Movie { get; set; }

        [InverseProperty("Screening")]
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
