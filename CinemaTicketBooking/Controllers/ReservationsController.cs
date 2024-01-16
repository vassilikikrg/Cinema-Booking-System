using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinemaTicketBooking.Data;
using CinemaTicketBooking.Models;
using CinemaTicketBooking.ViewModels;

namespace CinemaTicketBooking.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly CinemaDbContext _context;

        public ReservationsController(CinemaDbContext context)
        {
            _context = context;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            int customerId = 1; //HARDCODED FOR NOW
            var cinemaDbContext = _context.Reservations
                .Include(r => r.Customer)
                .Include(r => r.Screening)
                .ThenInclude(s => s.Movie)
                .Where(r=>r.CustomerId==customerId);
            return View(await cinemaDbContext.ToListAsync());
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            int customerId = 1; //HARDCODED FOR NOW
            var reservation = await _context.Reservations
                .Include(r => r.Customer)
                .Include(r => r.Screening)
                .FirstOrDefaultAsync(m => m.Id == id && m.CustomerId==customerId);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            ViewData["CustomerName"] = new SelectList(_context.Customers, "Id", "Name");
            var screening = _context.Screenings.Include(s => s.Movie);
            ViewData["ScreeningName"] = new SelectList(screening, "Id", "Movie.Name");
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ScreeningId,CustomerId,NumberOfSeats")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", reservation.CustomerId);
            ViewData["ScreeningId"] = new SelectList(_context.Screenings, "Id", "Id", reservation.ScreeningId);
            return View(reservation);
        }


        // GET: /Screening/SelectMovie
        public IActionResult SelectMovie()
        {
            var movies = _context.Movies.ToList();
            return View(movies);
        }

        // POST: /Screening/SelectMovie
        [HttpPost]
        public IActionResult SelectMovie(int selectedMovieId)
        {
            // Redirect to the action that displays screenings for the selected movie
            return RedirectToAction("SelectScreening", new { movieId = selectedMovieId });
        }

        // GET: /Screening/SelectScreening/{movieId}
        public IActionResult SelectScreening(int movieId)
        {
            var movie = _context.Movies.Find(movieId);

            if (movie == null)
            {
                return NotFound(); // Handle the case where the movie is not found
            }

            var screenings = _context.Screenings
                .Where(s => s.MovieId == movieId && s.StartDateAndTime>DateTime.Now)
                .OrderBy(s=> s.StartDateAndTime)
                .ToList();

            // Pass the movie and its screenings to the view model
            var viewModel = new SelectScreeningViewModel
            {
                Movie = movie,
                Screenings = screenings
            };

            return View(viewModel);
        }

        // POST: /Screening/SelectScreening/{movieId}
        [HttpPost]
        public IActionResult SelectScreening(int movieId, int selectedScreeningId)
        {
            // Redirect to the action that handles reservation for the selected screening
            return RedirectToAction("MakeReservation", new { screeningId = selectedScreeningId });
        }

        // GET: /Screening/MakeReservation/{screeningId}
        public IActionResult MakeReservation(int screeningId)
        {
            var screening = _context.Screenings
                .Include(s=>s.Movie)
                .Include(s=>s.Cinema)
                .FirstOrDefault(s => s.Id == screeningId);

            if (screening == null)
            {
                return NotFound(); // Handle the case where the screening is not found
            }
            MakeReservationViewModel viewModel = new MakeReservationViewModel {ScreeningId=screeningId,
                MovieName=screening.Movie.Name,
                StartDateAndTime=screening.StartDateAndTime,
                AvailableSeats=screening.AvailableSeats,
                NumberOfSeats=1};
            // Pass the screening information to the reservation view
            return View(viewModel);
        }

        [HttpPost]
        //POST: /Screening/MakeReservation/{screeningId}
        public IActionResult MakeReservation(int screeningId,int NumberOfSeats)
        {
            int customerId = 1;
            Reservation reservation = new Reservation()
                {
                    CustomerId = customerId, // Hardcoded for now
                    ScreeningId = screeningId,
                    NumberOfSeats = NumberOfSeats
                };

            _context.Add(reservation);
            //Update screening's available seats
            Screening screening=_context.Screenings.Find(screeningId);
            int availableSeats = screening.AvailableSeats;
            screening.AvailableSeats = availableSeats-NumberOfSeats;
            _context.Update(screening);

            _context.SaveChanges();

            return RedirectToAction("Index"); // Redirect to the appropriate action

        }
    // GET: Reservations/Edit/5
    public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", reservation.CustomerId);
            ViewData["ScreeningId"] = new SelectList(_context.Screenings, "Id", "Id", reservation.ScreeningId);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ScreeningId,CustomerId,NumberOfSeats")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", reservation.CustomerId);
            ViewData["ScreeningId"] = new SelectList(_context.Screenings, "Id", "Id", reservation.ScreeningId);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Customer)
                .Include(r => r.Screening)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
