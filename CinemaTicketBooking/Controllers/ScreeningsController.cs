using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinemaTicketBooking.Data;
using CinemaTicketBooking.Models;

namespace CinemaTicketBooking.Controllers
{
    public class ScreeningsController : Controller
    {
        private readonly CinemaDbContext _context;

        public ScreeningsController(CinemaDbContext context)
        {
            _context = context;
        }

        // GET: Screenings
        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate)
        {
            // Default time period if not provided
            startDate ??= DateTime.Now.Date;
            endDate ??= DateTime.Now.Date.AddDays(7); // Assuming a default period of one week

            var cinemaDbContext = _context.Screenings
                .Include(s => s.Cinema)
                .Include(s => s.Movie)
                .Where(s => s.StartDateAndTime >= startDate && s.StartDateAndTime <= endDate)
                .OrderBy(s=>s.StartDateAndTime);
            return View(await cinemaDbContext.ToListAsync());
        }

        // GET: Screenings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var screening = await _context.Screenings
                .Include(s => s.Cinema)
                .Include(s => s.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (screening == null)
            {
                return NotFound();
            }

            return View(screening);
        }

        // GET: Screenings/Create
        public IActionResult Create()
        {
            ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Name");
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Name");
            return View();
        }

        // POST: Screenings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MovieId,CinemaId,StartDateAndTime")] Screening screening)
        {
            bool error = false;
            if (ModelState.IsValid)
            {
                if (!ScreeningDateTimeIsValid(screening.StartDateAndTime))
                {
                    ModelState.AddModelError(nameof(Screening.StartDateAndTime), "Select a future date and starting time");
                    error = true;
                }
                var movie = _context.Movies.FirstOrDefault(m => m.Id == screening.MovieId);
                if (screeningsOverlapping(screening.MovieId, screening.CinemaId, screening.StartDateAndTime, movie.Length))
                {
                    ModelState.AddModelError(nameof(Screening.StartDateAndTime), "There is already a screening for this movie in the specified cinema hall starting or in progress at the time and day specified");
                    error = true;
                }
                if (error)
                {
                    ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Name", screening.CinemaId);
                    ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Name", screening.MovieId);
                    return View(screening);
                }
                else
                {
                    _context.Screenings.Add(screening);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Name", screening.CinemaId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Name", screening.MovieId);
            return View(screening);
        }

        // GET: Screenings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var screening = await _context.Screenings.FindAsync(id);
            if (screening == null)
            {
                return NotFound();
            }
            ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Name", screening.CinemaId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Name", screening.MovieId);
            return View(screening);
        }

        // POST: Screenings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MovieId,CinemaId,StartDateAndTime")] Screening screening)
        {
            if (id != screening.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    bool error = false;
                    if (!ScreeningDateTimeIsValid(screening.StartDateAndTime))
                    {
                        ModelState.AddModelError(nameof(Screening.StartDateAndTime), "Select a future date and starting time");
                        error = true;
                    }
                    var movie = _context.Movies.FirstOrDefault(m => m.Id == screening.MovieId);
                    if (screeningsOverlapping(screening.MovieId, screening.CinemaId, screening.StartDateAndTime, movie.Length, screening.Id))
                    {
                        ModelState.AddModelError(nameof(Screening.StartDateAndTime), "There is already a screening for this movie in the specified cinema hall starting or in progress at the time and day specified");
                        error = true;
                    }

                    if (error)
                    {
                        ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Name", screening.CinemaId);
                        ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Name", screening.MovieId);
                        return View(screening);
                    }
                    else
                    {
                        _context.Screenings.Update(screening);
                        await _context.SaveChangesAsync();
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScreeningExists(screening.Id))
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
            ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Name", screening.CinemaId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Name", screening.MovieId);
            return View(screening);
        }

        // GET: Screenings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var screening = await _context.Screenings
                .Include(s => s.Cinema)
                .Include(s => s.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (screening == null)
            {
                return NotFound();
            }

            return View(screening);
        }

        // POST: Screenings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var screening = await _context.Screenings.FindAsync(id);
            if (screening != null)
            {
                _context.Screenings.Remove(screening);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScreeningExists(int id)
        {
            return _context.Screenings.Any(e => e.Id == id);
        }

        public bool ScreeningDateTimeIsValid(DateTime date)
        {
            return !(date < DateTime.Now);
        }
        public bool screeningsOverlapping(int movieId, int cinemaId, DateTime screeningDate,int movieLength)
        {   //check if there is another screening scheduled for the same time period
            DateTime date1=screeningDate.AddMinutes(-movieLength);
            DateTime date2=screeningDate.AddMinutes(movieLength);
            return _context.Screenings
                .Any(e => e.MovieId == movieId && e.CinemaId==cinemaId && e.StartDateAndTime>date1 && e.StartDateAndTime<date2);
        }
        public bool screeningsOverlapping(int movieId, int cinemaId, DateTime screeningDate, int movieLength , int screeningId)
        {   //check if there is another screening scheduled for the same time period
            DateTime date1 = screeningDate.AddMinutes(-movieLength);
            DateTime date2 = screeningDate.AddMinutes(movieLength);
            return _context.Screenings
                .Any(e => e.Id != screeningId && e.MovieId == movieId && e.CinemaId == cinemaId && e.StartDateAndTime > date1 && e.StartDateAndTime < date2);
        }
    }
}
