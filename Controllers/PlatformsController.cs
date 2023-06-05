using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backlogged_api.Data;
using backlogged_api.Models;
using backlogged_api.DTO.Platform;
using backlogged_api.Helpers;
using Newtonsoft.Json;
using backlogged_api.DTO.Game;

namespace backlogged_api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class PlatformsController : ControllerBase
    {
        private readonly BackloggedDBContext _context;

        public PlatformsController(BackloggedDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all platforms.
        /// </summary>
        /// <returns>All platforms</returns>
        /// <response code="200">Returns the platforms correctly</response>
        // GET: api/Platforms
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PlatformDto>>> GetAllPlatforms([FromQuery] PagingParams pagingParams)
        {
            if (_context.Platforms == null)
            {
                return NotFound();
            }
            var platforms = await PageListBuilder.CreatePagedListAsync(_context.Platforms.Select(s => new PlatformDto
            {
                id = s.Id,
                name = s.Name
            }), m => m.name, pagingParams.PageNumber, pagingParams.PageSize);

            var metadata = new
            {
                platforms.TotalCount,
                platforms.PageSize,
                platforms.CurrentPage,
                platforms.TotalPages,
                platforms.HasNext,
                platforms.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(platforms);
        }

        /// <summary>
        /// Gets a platform based on its' id.
        /// </summary>
        /// <returns>platform</returns>
        /// <response code="200">Returns the platform</response>
        /// <response code="404">platform not found</response>
        // GET: api/Platforms/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Platform>> GetPlatform(Guid id)
        {
            if (_context.Platforms == null)
            {
                return NotFound();
            }
            var platform = await _context.Platforms
            .Where(w => w.Id == id)
            .Select(s => new PlatformDto
            {
                id = s.Id,
                name = s.Name
            }).FirstOrDefaultAsync();

            if (platform == null)
            {
                return NotFound();
            }

            return Ok(platform);
        }

        /// <summary>
        /// Updates a platform based on its' id.
        /// </summary>
        /// <returns>platform</returns>
        /// <response code="204">platform updated, no response</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">platform not found</response>
        // PUT: api/Platforms/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutPlatform(Guid id, UpdatePlatformDto platformDto)
        {

            if (!_context.Platforms.Any(a => a.Id == id))
            {
                return NotFound();
            }

            var platform = new Platform
            {
                Id = id,
                Name = platformDto.name
            };
            _context.Entry(platform).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlatformExists(id))
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
        /// Adds a new platform to the store.
        /// </summary>
        /// <returns>platform</returns>
        /// <response code="201">Returns the platform</response>
        /// <response code="400">Bad request</response>
        // POST: api/Platforms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PlatformDto>> PostPlatform(CreatePlatformDto platformDto)
        {
            if (_context.Platforms == null)
            {
                return Problem("Entity set 'BackloggedDBContext.Platform'  is null.");
            }
            var platform = new Platform
            {
                Name = platformDto.name
            };
            _context.Platforms.Add(platform);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPlatform), new { id = platform.Id }, new PlatformDto { id = platform.Id, name = platform.Name });
        }

        /// <summary>
        /// Deletes a platform from the store.
        /// </summary>
        /// <returns>platform</returns>
        /// <response code="204">platform deleted, no response</response>
        /// <response code="404">platform not found</response>
        // DELETE: api/Platforms/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePlatform(Guid id)
        {
            if (_context.Platforms == null)
            {
                return NotFound();
            }
            var platform = await _context.Platforms.FindAsync(id);
            if (platform == null)
            {
                return NotFound();
            }

            _context.Platforms.Remove(platform);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Gets the games for a platform.
        /// </summary>
        /// <returns>backlog</returns>
        /// <response code="200">Games</response>
        /// <response code="404">platform not found</response>
        // Get: api/platform/uuid/Games
        [HttpGet("{id}/Games")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGames(Guid id, [FromQuery] PagingParams pagingParams)
        {
            if (_context.Publishers == null)
            {
                return NotFound();
            }

            var games = await PageListBuilder.CreatePagedListAsync(_context.Platforms
                .Include(i => i.Games)
                .Where(w => w.Id == id)
                .SelectMany(s => s.Games.Select(s => new GameDto
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
                })), m => m.Title, pagingParams.PageNumber, pagingParams.PageSize);

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

        private bool PlatformExists(Guid id)
        {
            return (_context.Platforms?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
