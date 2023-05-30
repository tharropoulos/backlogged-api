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

namespace backlogged_api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ReviewsController : ControllerBase
    {
        private readonly BackloggedDBContext _context;

        public ReviewsController(BackloggedDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all reviews.
        /// </summary>
        /// <returns>All reviews</returns>
        /// <response code="200">Returns the reviews correctly</response>
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
                Id = p.id,
                Rating = p.rating,
                GameId = p.gameId,
                AuthorId = p.authorId,
                Details = p.details,
                CreatedAt = p.createdAt,

            }).ToListAsync();
            return Ok(reviews);
        }

        /// <summary>
        /// Gets a review based on its' id.
        /// </summary>
        /// <returns>review</returns>
        /// <response code="200">Returns the review</response>
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
            .Where(w => w.id == id)
            .Select(s => new ReviewDto
            {
                Id = s.id,
                Rating = s.rating,
                GameId = s.gameId,
                AuthorId = s.authorId,
                Details = s.details,
                CreatedAt = s.createdAt,

            }).FirstOrDefaultAsync();

            if (review == null)
            {
                return NotFound();
            }

            return Ok(review);
        }

        /// <summary>
        /// Updates a review based on its' id.
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
                .FirstOrDefaultAsync(m => m.id == id);

            if (reviewToUpdate == null)
            {
                return NotFound();
            }

            reviewToUpdate.rating = reviewDto.Rating;
            reviewToUpdate.details = reviewDto.Details;

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
        /// Adds a new review to the store.
        /// </summary>
        /// <returns>review</returns>
        /// <response code="201">Returns the review</response>
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
            if (!_context.Games.Any(g => g.id == gameId))
            {
                return BadRequest("Game does not exist.");
            }
            if (!_context.Users.Any(u => u.id == authorId))
            {
                return BadRequest("User does not exist.");
            }
            var review = new Review
            {
                rating = reviewDto.Rating,
                details = reviewDto.Details,
                gameId = gameId,
                authorId = authorId,
                createdAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)
            };
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetReview), new { id = review.id }, new ReviewDto { Id = review.id, Rating = review.rating, Details = review.details, GameId = review.gameId, AuthorId = review.authorId, CreatedAt = review.createdAt });
        }

        /// <summary>
        /// Deletes a review from the store.
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
            return (_context.Reviews?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
