using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.ViewModels
{
    public class CreateMovieViewModel
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string Content { get; set; }

        [Required]
        [Range(1, 1000)]
        public int Length { get; set; }

        [Required]
        [StringLength(255)]
        public string Summary { get; set; }

        [Required]
        [StringLength(255)]
        public string Director { get; set; }
    }

}
