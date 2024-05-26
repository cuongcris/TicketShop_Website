using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketOnline.Data;
using TicketOnline.Model;

namespace TicketOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public MoviesController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {

            return await _context.Movies.Include(m => m.Showtimes).ToListAsync();
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(string id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }

        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutMovie(string id, MovieCreate movie)
        {
            var movie1 = await _context.Movies.FindAsync(id);

            if (movie1 == null) { return NotFound(); }

            if (!string.IsNullOrEmpty(movie.Title))
                movie1.Title = movie.Title;
            if (!string.IsNullOrEmpty(movie.Name))
                movie1.Name = movie.Name;
            if (!string.IsNullOrEmpty(movie.Director))
                movie1.Director = movie.Director;
            if (!string.IsNullOrEmpty(movie.Cast))
                movie1.Cast = movie.Cast;
            if (!string.IsNullOrEmpty(movie.GenreId))
                movie1.GenreId = movie.GenreId;
            if (!string.IsNullOrEmpty(movie.Language))
                movie1.Language = movie.Language;
            if (!string.IsNullOrEmpty(movie.ReleaseDate))
                movie1.ReleaseDate = DateOnly.Parse(movie.ReleaseDate);
            if (!string.IsNullOrEmpty(movie.RunTime))
                movie1.RunTime = TimeOnly.Parse(movie.RunTime);
            if (!string.IsNullOrEmpty(movie.Tagline))
                movie1.Tagline = movie1.Tagline;
            if (!string.IsNullOrEmpty(movie.HomePage))
                movie1.HomePage = movie.HomePage;
            if (!string.IsNullOrEmpty(movie.Overview))
                movie1.Overview = movie1.Overview;
            if (!string.IsNullOrEmpty(movie.PosterPath))
                movie1.PosterPath = movie.PosterPath;
            if (!string.IsNullOrEmpty(movie.BackdropPath))
                movie1.BackdropPath = movie.BackdropPath;
            if (movie.Status != null) movie1.Status = (bool)movie.Status;
            if (movie.Vote_count != null) movie1.Vote_count = (int)movie.Vote_count;
            if (movie.Video != null) movie1.Video = (bool)movie.Video;
            if (movie.Adult != null) movie1.Adult = (bool)movie.Adult;
            if (movie.Revenue != null) movie1.Revenue = (int)movie.Revenue;
            if (movie.Vote_average != null) movie1.Vote_average = (int)movie.Vote_average;
            if (movie.Popularity != null) movie1.Popularity = (int)movie.Popularity;






            _context.Entry(movie1).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(movie1);
        }

        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<Movie>> PostMovie(MovieCreate movie)
        {
            try
            {

                Movie movie1 = new Movie()
                {
                    Name = movie.Name,
                    Title = movie.Title,
                    Director = movie.Director,
                    GenreId = movie.GenreId,
                    Cast = movie.Cast,
                    ReleaseDate = DateOnly.Parse(movie.ReleaseDate),
                    RunTime = TimeOnly.Parse(movie.RunTime),
                    Adult = (bool)movie.Adult,
                    Vote_average = (decimal)movie.Vote_average,
                    BackdropPath = movie.BackdropPath,
                    Budget = (int)movie.Budget,
                    HomePage = movie.HomePage,
                    Language = movie.Language,
                    Overview = movie.Overview,
                    Popularity = (decimal)movie.Popularity,
                    PosterPath = movie.PosterPath,
                    Revenue = (int)movie.Revenue,
                    Status = (bool)movie.Status,
                    Tagline = movie.Tagline,
                    Video = (bool)movie.Video,
                    Vote_count = (int)movie.Vote_count
                };

                _context.Movies.Add(movie1);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetMovie", new { id = movie1.Id }, movie1);
            }
            catch (Exception ex)
            {
                //Log out the error
                Console.WriteLine(ex.ToString());
                throw ex;
            }
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMovie(string id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(string id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
