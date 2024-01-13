using System;
using System.Collections.Generic;
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
        public int MovieId { get; set; }

        [Column("cinema_id")]
        public int CinemaId { get; set; }

        [Column("day")]
        public DateTime Day { get; set; } 

        [Column("start_time")]
        public TimeSpan StartTime { get; set; } 

        [ForeignKey("CinemaId")]
        [InverseProperty("Screenings")]
        public virtual Cinema Cinema { get; set; } = null!;

        [ForeignKey("MovieId")]
        [InverseProperty("Screenings")]
        public virtual Movie Movie { get; set; } = null!;

        [InverseProperty("Screening")]
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
