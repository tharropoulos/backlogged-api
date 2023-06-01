using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backlogged_api.Data;
using backlogged_api.Models;
using backlogged_api.DTO.Genre;

namespace backlogged_api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class GenresController : ControllerBase
    {
        private readonly BackloggedDBContext _context;

        public GenresController(BackloggedDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all genres.
        /// </summary>
        /// <returns>All genres</returns>
        /// <response code="200">Returns the genres correctly</response>
        // GET: api/Genres
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GenreDto>>> GetAllGenres()
        {
            if (_context.Genres == null)
            {
                return NotFound();
            }
            var genres = await _context.Genres.Select(p => new GenreDto
            {
                id = p.Id,
                name = p.Name
            }).ToListAsync();
            return Ok(genres);
        }

        /// <summary>
        /// Gets a genre based on its' id.
        /// </summary>
        /// <returns>genre</returns>
        /// <response code="200">Returns the genre</response>
        /// <response code="404">genre not found</response>
        // GET: api/Genres/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GenreDto>> GetGenre(Guid id)
        {
            if (_context.Genres == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres
            .Where(w => w.Id == id)
            .Select(s => new GenreDto
            {
                id = s.Id,
                name = s.Name
            }).FirstOrDefaultAsync();

            if (genre == null)
            {
                return NotFound();
            }

            return Ok(genre);
        }

        /// <summary>
        /// Updates a genre based on its' id.
        /// </summary>
        /// <returns>genre</returns>
        /// <response code="204">genre updated, no response</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">genre not found</response>
        // PUT: api/Genres/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutGenre(Guid id, UpdateGenreDto genreDto)
        {

            if (!_context.Genres.Any(a => a.Id == id))
            {
                return NotFound();
            }

            var genre = new Genre
            {
                Id = id,
                Name = genreDto.name
            };
            _context.Entry(genre).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GenreExists(id))
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

        /// <summary>
        /// Adds a new genre to the store.
        /// </summary>
        /// <returns>genre</returns>
        /// <response code="201">Returns the genre</response>
        /// <response code="400">Bad request</response>
        // POST: api/Genres
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GenreDto>> Postgenre(CreateGenreDto genreDto)
        {
            if (_context.Genres == null)
            {
                return Problem("Entity set 'BackloggedDBContext.Genre'  is null.");
            }
            var genre = new Genre
            {
                Name = genreDto.name
            };
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetGenre), new { id = genre.Id }, new GenreDto { id = genre.Id, name = genre.Name });
        }

        /// <summary>
        /// Deletes a genre from the store.
        /// </summary>
        /// <returns>genre</returns>
        /// <response code="204">genre deleted, no response</response>
        /// <response code="404">genre not found</response>
        // DELETE: api/Genres/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteGenre(Guid id)
        {
            if (_context.Genres == null)
            {
                return NotFound();
            }
            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
            {
                return NotFound();
            }

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GenreExists(Guid id)
        {
            return (_context.Genres?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
