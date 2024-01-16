using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBooking.Models;

[Table("reservations")]
public partial class Reservation
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("screening_id")]
    public int ScreeningId { get; set; }

    [Column("customer_id")]
    public int CustomerId { get; set; }

    [Column("number_of_seats")]
    [DisplayName("Number of seats reserved")]
    [Range(1,20)]
    public int NumberOfSeats { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Reservations")]
    public virtual Customer Customer { get; set; } = null!;

    [ForeignKey("ScreeningId")]
    [InverseProperty("Reservations")]
    public virtual Screening Screening { get; set; } = null!;
}
