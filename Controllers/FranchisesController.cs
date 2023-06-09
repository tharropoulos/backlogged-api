using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backlogged_api.Data;
using backlogged_api.Models;
using backlogged_api.DTO.Franchise;
using backlogged_api.Helpers;
using Newtonsoft.Json;
using backlogged_api.DTO.Game;
using Microsoft.AspNetCore.Authorization;

namespace backlogged_api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class FranchiseController : ControllerBase
    {
        private readonly BackloggedDBContext _context;

        public FranchiseController(BackloggedDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all Franchises.
        /// </summary>
        /// <returns>All Franchises</returns>
        /// <response code="200">Returns the Franchises correctly</response>
        // GET: api/Franchises
        [MapToApiVersion("1.0")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<FranchiseDto>>> GetAllFranchises([FromQuery] PagingParams pagingParams)
        {
            if (_context.Franchises == null)
            {
                return NotFound();
            }
            var franchises = await PageListBuilder.CreatePagedListAsync(_context.Franchises.Select(s => new FranchiseDto
            {
                id = s.Id,
                name = s.Name
            }), m => m.name, pagingParams.PageNumber, pagingParams.PageSize);

            var metadata = new
            {
                franchises.TotalCount,
                franchises.PageSize,
                franchises.CurrentPage,
                franchises.TotalPages,
                franchises.HasNext,
                franchises.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(franchises);
        }

        /// <summary>
        /// Gets a Franchise based on its' id.
        /// </summary>
        /// <returns>Franchise</returns>
        /// <response code="200">Returns the Franchise</response>
        /// <response code="404">Franchise not found</response>
        // GET: api/Franchises/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FranchiseDto>> GetFranchise(Guid id)
        {
            if (_context.Franchises == null)
            {
                return NotFound();
            }
            var franchise = await _context.Franchises
            .Where(w => w.Id == id)
            .Select(s => new FranchiseDto
            {
                id = s.Id,
                name = s.Name
            }).FirstOrDefaultAsync();

            if (franchise == null)
            {
                return NotFound();
            }

            return franchise;
        }

        /// <summary>
        /// Updates a Franchise's details.
        /// </summary>
        /// <response code="204">Franchise updated, no content</response>
        /// <response code="404">Franchise not Franchise</response>
        /// <response code="400">Bad request</response>
        // PUT: api/Franchises/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutFranchise(Guid id, UpdateFranchiseDto franchiseDto)
        {
            if (!_context.Franchises.Any(a => a.Id == id))
            {
                return NotFound();
            }

            var franchise = new Franchise { Name = franchiseDto.name, Id = id };
            _context.Entry(franchise).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FranchiseExists(id))
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
        /// Adds a new Franchise to the store.
        /// </summary>
        /// <returns>Franchise</returns>
        /// <response code="200">Franchise created</response>
        /// <response code="400">Bad request</response>
        // POST: api/Franchises
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FranchiseDto>> PostFranchise(CreateFranchiseDto franchiseDto)
        {
            if (_context.Franchises == null)
            {
                return Problem("Entity set 'BackloggedDBContext.Franchises'  is null.");
            }
            var franchise = new Franchise
            {
                Name = franchiseDto.name
            };
            _context.Franchises.Add(franchise);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFranchise), new { id = franchise.Id }, new FranchiseDto { id = franchise.Id, name = franchise.Name });
        }
        /// <summary>
        /// Deletes a Franchise from the store.
        /// </summary>
        /// <returns>Franchise</returns>
        /// <response code="204">Franchise deleted, no content</response>
        /// <response code="404">Franchise not found</response>

        // DELETE: api/Franchises/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFranchise(Guid id)
        {
            if (_context.Franchises == null)
            {
                return NotFound();
            }
            var franchise = await _context.Franchises.FindAsync(id);
            if (franchise == null)
            {
                return NotFound();
            }

            _context.Franchises.Remove(franchise);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Gets the games for a Franchise.
        /// </summary>
        /// <returns>backlog</returns>
        /// <response code="200">Games</response>
        /// <response code="404">Franchise not found</response>
        // Get: api/franchise/uuid/Games
        [HttpGet("{id}/Game")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGames(Guid id, [FromQuery] PagingParams pagingParams)
        {
            if (_context.Franchises == null)
            {
                return NotFound();
            }

            var games = await PageListBuilder.CreatePagedListAsync(_context.Games.Where(w => w.FranchiseId == id).Select(s => new GameDto
            {
                Title = s.Title,
                BackgoundImageUrl = s.BackgroundImageUrl,
                Id = s.Id,
                ReleaseDate = s.ReleaseDate,
                CoverImageUrl = s.CoverImageUrl,
                Description = s.Description,
                FranchiseId = s.FranchiseId,
                PublisherId = s.PublisherId,
                Rating = s.Rating,
            }), m => m.Title, pagingParams.PageNumber, pagingParams.PageSize);


            var metadata = new
            {
                games.TotalCount,
                games.PageSize,
                games.CurrentPage,
                games.TotalPages,
                games.HasNext,
                games.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(games);
        }
        private bool FranchiseExists(Guid id)
        {
            return (_context.Franchises?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
