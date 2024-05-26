using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketOnline.Data;

namespace TicketOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatsController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public SeatsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Seats
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Seat>>> GetSeats()
        {
            return await _context.Seats.ToListAsync();
        }

        // GET: api/Seats/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Seat>> GetSeat(string id)
        {
            var seat = await _context.Seats.FindAsync(id);

            if (seat == null)
            {
                return NotFound();
            }

            return seat;
        }

        // PUT: api/Seats/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutSeat(string id, Seat seat)
        {
            if (id != seat.Id)
            {
                return BadRequest();
            }

            _context.Entry(seat).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SeatExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Seats
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<Seat>> PostSeat(Seat seat)
        {
            _context.Seats.Add(seat);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSeat", new { id = seat.Id }, seat);
        }

        // DELETE: api/Seats/5
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSeat(int id)
        {
            var seat = await _context.Seats.FindAsync(id);
            if (seat == null)
            {
                return NotFound();
            }

            _context.Seats.Remove(seat);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SeatExists(string id)
        {
            return _context.Seats.Any(e => e.Id == id);
        }
        [HttpGet("Showtime/{showtimeid}")]
        public async Task<IEnumerable<Seat>> GetSeatByShowTime(string showtimeid)
        {
            var showtime = await _context.ShowTimes.FindAsync(showtimeid);
            var seatsForShowtime = _context.Seats
        .Where(s => s.RoomNumberId == showtime.RoomNumberId && s.Tickets.Any(t => t.ShowtimeId == showtimeid))
        .ToList();
            return seatsForShowtime;
        }
        [HttpPost("addSeat")]
        public async Task<bool> AddFullSeat()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();

            for (int k = 1; k <= 5; k++)
            {
                for (char c = 'A'; c <= 'M'; c++)
                {
                    for (int i = 1; i <= 10; i++)
                    {
                        Seat seat = new Seat()
                        {
                            Id = new string(Enumerable.Repeat(chars, 30)
                    .Select(s => s[random.Next(s.Length)]).ToArray()),
                            RoomNumberId = k,
                            SeatNumber = i,
                            RowName = c,
                        };
                        _context.Seats.Add(seat);
                    }
                }
            }
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
