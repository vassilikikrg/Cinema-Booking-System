using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBooking.Models;

[Table("movies")]
public partial class Movie
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(255)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("content")]
    [StringLength(255)]
    [Unicode(false)]
    public string Content { get; set; } = null!;

    [Column("length")]
    [Display(Name = "Length ( in minutes )")]
    public int Length { get; set; }

    [Column("summary")]
    [StringLength(255)]
    [Unicode(false)]
    public string Summary { get; set; } = null!;

    [Column("director")]
    [StringLength(255)]
    [Unicode(false)]
    public string Director { get; set; } = null!;

    [Column("content_admin_id")]
    public int ContentAdminId { get; set; }

    [ForeignKey("ContentAdminId")]
    [InverseProperty("Movies")]
    public virtual ContentAdmin ContentAdmin { get; set; } = null!;

    [InverseProperty("Movie")]
    public virtual ICollection<Screening> Screenings { get; set; } = new List<Screening>();
}
