using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backlogged_api.Data;
using backlogged_api.Models;
using backlogged_api.DTO.Review;
using Microsoft.AspNetCore.Authorization;

namespace backlogged_api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ReviewController : ControllerBase
    {
        private readonly BackloggedDBContext _context;

        public ReviewController(BackloggedDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all Reviews.
        /// </summary>
        /// <returns>All Reviews</returns>
        /// <response code="200">Returns the Reviews correctly</response>
        // GET: api/Reviews
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetAllReviews()
        {
            if (_context.Reviews == null)
            {
                return NotFound();
            }
            var reviews = await _context.Reviews.Select(p => new ReviewDto
            {
                Id = p.Id,
                Rating = p.Rating,
                GameId = p.GameId,
                AuthorId = p.AuthorId,
                Details = p.Details,
                CreatedAt = p.CreatedAt,

            }).ToListAsync();
            return Ok(reviews);
        }

        /// <summary>
        /// Gets a Review based on its' id.
        /// </summary>
        /// <returns>review</returns>
        /// <response code="200">Returns the Reviews </response>
        /// <response code="404">review not found</response>
        // GET: api/Reviews/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReviewDto>> GetReview(Guid id)
        {
            if (_context.Reviews == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
            .Where(w => w.Id == id)
            .Select(s => new ReviewDto
            {
                Id = s.Id,
                Rating = s.Rating,
                GameId = s.GameId,
                AuthorId = s.AuthorId,
                Details = s.Details,
                CreatedAt = s.CreatedAt,

            }).FirstOrDefaultAsync();

            if (review == null)
            {
                return NotFound();
            }

            return Ok(review);
        }

        /// <summary>
        /// Updates a Review based on its' id.
        /// </summary>
        /// <returns>review</returns>
        /// <response code="204">review updated, no response</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">review not found</response>
        // PUT: api/Reviews/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutReview(Guid id, UpdateReviewDto reviewDto)
        {

            var reviewToUpdate = await _context.Reviews
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (reviewToUpdate == null)
            {
                return NotFound();
            }

            reviewToUpdate.Rating = reviewDto.Rating;
            reviewToUpdate.Details = reviewDto.Details;

            _context.Entry(reviewToUpdate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
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
        /// Adds a new review to Backlogged.
        /// </summary>
        /// <returns>review</returns>
        /// <response code="201">Returns the Reviews </response>
        /// <response code="400">Bad request</response>
        // POST: api/Reviews
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ReviewDto>> Postreview([FromQuery] Guid authorId, [FromQuery] Guid gameId, CreateReviewDto reviewDto)
        {
            if (_context.Reviews == null)
            {
                return Problem("Entity set 'BackloggedDBContext.Review'  is null.");
            }
            if (!_context.Games.Any(g => g.Id == gameId))
            {
                return BadRequest("Game does not exist.");
            }
            if (!_context.Users.Any(u => u.Id == authorId))
            {
                return BadRequest("User does not exist.");
            }
            var review = new Review
            {
                Rating = reviewDto.Rating,
                Details = reviewDto.Details,
                GameId = gameId,
                AuthorId = authorId,
                // could be redudant, but just in case
                CreatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)
            };
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetReview), new { id = review.Id }, new ReviewDto
            {
                Id = review.Id,
                Rating = review.Rating,
                Details = review.Details,
                GameId = review.GameId,
                AuthorId = review.AuthorId,
                CreatedAt = review.CreatedAt
            });
        }

        /// <summary>
        /// Deletes a Review from Backlogged.
        /// </summary>
        /// <returns>review</returns>
        /// <response code="204">review deleted, no response</response>
        /// <response code="404">review not found</response>
        // DELETE: api/Reviews/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteReview(Guid id)
        {
            if (_context.Reviews == null)
            {
                return NotFound();
            }
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReviewExists(Guid id)
        {
            return (_context.Reviews?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
