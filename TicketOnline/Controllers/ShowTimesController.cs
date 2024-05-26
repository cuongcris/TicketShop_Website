using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using TicketOnline.Data;
using TicketOnline.Model;

namespace TicketOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowTimesController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public ShowTimesController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/ShowTimes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShowTime>>> GetShowTimes()
        {
            return await _context.ShowTimes.ToListAsync();
        }

        // GET: api/ShowTimes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ShowTime>> GetShowTime(string id)
        {
            var showTime = await _context.ShowTimes.FindAsync(id);

            if (showTime == null)
            {
                return NotFound();
            }

            return showTime;
        }

        // PUT: api/ShowTimes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutShowTime(string id, ShowTimeCreate showTime)
        {
            var showtime2 = await _context.ShowTimes.FindAsync(id);

            if(showtime2 == null) { return NotFound(); }

            var showtime1 = showtime2;


            if(showTime.RoomNumberId != null)
                if(showTime.RoomNumberId != showtime1.RoomNumberId) { showtime1.RoomNumberId = (int)showTime.RoomNumberId; }
            if (showTime.StartTime != null)
                if (DateTime.Parse(showTime.StartTime).ToUniversalTime() != showtime1.StartTime) { 
                    showtime1.StartTime = DateTime.Parse(showTime.StartTime).ToUniversalTime();
                    var movie = await _context.Movies.FindAsync(showtime1.MovieId);
                    if(movie != null)
                    showtime1.EndTime = DateTime.Parse(showTime.StartTime).Add(movie.RunTime.ToTimeSpan()).ToUniversalTime();
                }
            if (showTime.MovieId != null)
                if (showTime.MovieId != showtime1.MovieId) { showtime1.MovieId = showTime.MovieId; }

            var check = await _context.ShowTimes.FirstOrDefaultAsync(s => s.RoomNumberId == showtime2.RoomNumberId && ((s.StartTime.CompareTo(showtime2.StartTime) < 0 &&
                                                            s.EndTime.CompareTo(showtime2.StartTime) >= 0 && s.Id != showtime2.Id) ||
                                                            (s.StartTime.CompareTo(showtime2.EndTime) < 0 &&
                                                            s.EndTime.CompareTo(showtime2.EndTime) >= 0 && s.Id != showtime2.Id)));
            if (check != null)
                return BadRequest();

            showtime2 = showtime1 as ShowTime;

            _context.Entry(showtime2).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShowTimeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(showtime2);
        }

        // POST: api/ShowTimes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<ShowTime>> PostShowTime(ShowTimeCreate showTime)
        {
            var movie = await _context.Movies.FindAsync(showTime.MovieId);

            ShowTime showTime1 = new ShowTime()
            {
                MovieId = showTime.MovieId,
                RoomNumberId = (int)showTime.RoomNumberId,
                StartTime = DateTime.Parse(showTime.StartTime).ToUniversalTime(),
                EndTime = DateTime.Parse(showTime.StartTime).Add(movie.RunTime.ToTimeSpan()).ToUniversalTime()
            };
            var check = await _context.ShowTimes.FirstOrDefaultAsync(s => s.RoomNumberId == showTime1.RoomNumberId && ( (s.StartTime.CompareTo(showTime1.StartTime) < 0 &&
                                                            s.EndTime.CompareTo(showTime1.StartTime) >= 0) || 
                                                            (s.StartTime.CompareTo(showTime1.EndTime) < 0 &&
                                                            s.EndTime.CompareTo(showTime1.EndTime) >= 0)));
            if(check != null) 
                return BadRequest();
            Console.WriteLine(check);
            _context.ShowTimes.Add(showTime1);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShowTime", new { id = showTime1.Id }, showTime);
        }

        // DELETE: api/ShowTimes/5
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteShowTime(string id)
        {
            var showTime = await _context.ShowTimes.FindAsync(id);
            if (showTime == null)
            {
                return NotFound();
            }

            _context.ShowTimes.Remove(showTime);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ShowTimeExists(string id)
        {
            return _context.ShowTimes.Any(e => e.Id == id);
        }
        [HttpGet("movie/{id}")]
        public IEnumerable<ShowTime> GetShowTimeByMovieId(string movieId)
        {
            var showtimes = _context.ShowTimes.Where(s => s.MovieId == movieId).ToList();
            return showtimes;
        }
        [HttpGet("customer")]
        public IEnumerable<ShowTime> GetShowTimeForWeek()
        {
            DateTime today = DateTime.Today;
            DateTime weekAfter = today.AddDays(14);

            var showtimes = _context.ShowTimes
                                  .Where(s => s.StartTime.CompareTo(today.ToUniversalTime()) >= 0 && s.StartTime.CompareTo(weekAfter.ToUniversalTime()) <= 0)
                                  .ToList();
            return showtimes;
        }
    }
}
