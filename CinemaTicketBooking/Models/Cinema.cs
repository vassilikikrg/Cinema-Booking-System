using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBooking.Models;

[Table("cinemas")]
public partial class Cinema
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(255)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("seats")]
    public int Seats { get; set; }

    [Column("3d")]
    [StringLength(255)]
    [Unicode(false)]
    public string _3d { get; set; } = null!;

    [InverseProperty("Cinema")]
    public virtual ICollection<Screening> Screenings { get; set; } = new List<Screening>();
}
