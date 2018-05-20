using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SAMoviesAPI.Contexts;
using SAMoviesAPI.Models;

namespace SAMoviesAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/movies")]
    public class MoviesController : Controller
    {
        private readonly MovieContext _context;

        public MoviesController(MovieContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns the list of the Movies.
        /// </summary>
        [HttpGet]
        public IEnumerable<Movie> GetMovies()
        {
            return _context.Movies.Include(m => m.Comments)
                .Include(m => m.Ratings).AsNoTracking();
        }

        /// <summary>
        /// Returns the average rating of a specific movie.
        /// </summary>
        /// <param name="id"></param>
        [ProducesResponseType(typeof(String), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [HttpGet("{id}/rating")]
        public async Task<IActionResult> GetMovieAverageRating([FromRoute] int id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movie = await _context.Movies.Include(m => m.Ratings).SingleOrDefaultAsync(m => m.Id == id);
            

            if (movie == null)
            {
                return NotFound();
            }
            var movieAverageRating = movie.GetAverageRating();
            if(movieAverageRating == -1.0)
            {
                return NoContent();
            }
            return Json(movie.GetAverageRating());
        }

        /// <summary>
        /// Returns the list of the Movies sorted by rating.
        /// </summary>
        [ProducesResponseType(typeof(IEnumerable<Movie>), 200)]
        [HttpGet("sorted")]
        public IActionResult GetMoviesSorted()
        {
            return Ok(SortedMoviesbyRating());
        }

        /// <summary>
        /// Find a Movie by id.
        /// </summary>
        /// <param name="id"></param>
        [ProducesResponseType(typeof(Movie), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovie([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movie = await _context.Movies.Include(m=>m.Comments)
                .Include(m=>m.Ratings).AsNoTracking().SingleOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }

        /// <summary>
        /// Update an existing Movie.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="movie"></param>
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie([FromRoute] int id, [FromBody] Movie movie)
        {
            if (!ModelState.IsValid || movie == null)
            {
                return BadRequest(ModelState);
            }

            if (id != movie.Id)
            {
                return BadRequest();
            }

            _context.Entry(movie).State = EntityState.Modified;

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
                    return BadRequest(Json("Couldn't update item!"));
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Creates a Movie.
        /// </summary>
        /// <param name="movie"></param>
        /// <returns>A newly created Movie</returns>
        [ProducesResponseType(typeof(Movie), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [HttpPost]
        public async Task<IActionResult> PostMovie([FromBody] Movie movie)
        {
            try {

                if (!ModelState.IsValid || movie == null)
                {
                    return BadRequest(ModelState);
                }
                bool itemExists = _context.Movies.Any(m => m.Id == movie.Id);
                if (itemExists)
                {
                    return StatusCode(StatusCodes.Status409Conflict, Json("Movie with the specific id already exists!"));
                }
                _context.Movies.Add(movie);
                await _context.SaveChangesAsync();


            }
            catch (Exception)
            {
                return BadRequest(Json("Couldn't create item!"));
            }

            return CreatedAtAction("GetMovie", new { id = movie.Id }, movie);

        }

        /// <summary>
        /// Deletes a Movie.
        /// </summary>
        /// <param name="id"></param>
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var movie = await _context.Movies.SingleOrDefaultAsync(m => m.Id == id);
                if (movie == null)
                {
                    return NotFound();
                }

                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest(Json("Couldn't delete item!"));
            }
            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
        private IEnumerable<Movie> SortedMoviesbyRating()
        {
            IEnumerable<Movie> sortedMovies = _context.Movies.Include(m=>m.Ratings).Include(n=>n.Comments).AsNoTracking();
            sortedMovies.OrderByDescending(s => s.GetAverageRating());
            return sortedMovies;
        }
    }
}