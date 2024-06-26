﻿using CinemaTicketBooking.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.ViewModels
{
    public class MakeReservationViewModel
    {
        //public Screening Screening { get; set; }
        public string MovieName { get; set; }
        public DateTime StartDateAndTime {  get; set; }
        public int ScreeningId { get; set; }

        [DisplayName("Number of seats")]
        public int NumberOfSeats {  get; set; }
        public int AvailableSeats { get; set; }
    }
}
