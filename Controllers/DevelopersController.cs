using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backlogged_api.Data;
using backlogged_api.Models;
using backlogged_api.DTO.Developer;
using backlogged_api.Helpers;
using Newtonsoft.Json;
using backlogged_api.DTO.Game;

namespace backlogged_api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class DevelopersController : ControllerBase
    {
        private readonly BackloggedDBContext _context;

        public DevelopersController(BackloggedDBContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Gets all developers.
        /// </summary>
        /// <returns>All developers</returns>
        /// <response code="200">Returns the developers correctly</response>
        // GET: api/Developers
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<DeveloperDto>>> GetAllDevelopers([FromQuery] PagingParams pagingParams)
        {
            if (_context.Developers == null)
            {
                return NotFound();
            }
            var developers = await PageListBuilder.CreatePagedListAsync(_context.Developers.Select(s => new DeveloperDto
            {
                id = s.Id,
                name = s.Name
            }), m => m.name, pagingParams.PageNumber, pagingParams.PageSize);
            var metadata = new
            {
                developers.TotalCount,
                developers.PageSize,
                developers.CurrentPage,
                developers.TotalPages,
                developers.HasNext,
                developers.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(developers);
        }
        /// <summary>
        /// Gets a developer based on its' id.
        /// </summary>
        /// <returns>Developer</returns>
        /// <response code="200">Returns the developer</response>
        /// <response code="404">Developer not found</response>
        // GET: api/Developers/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DeveloperDto>> GetDeveloper(Guid id)
        {
            if (_context.Developers == null)
            {
                return NotFound();
            }
            var developer = await _context.Developers
            .Where(w => w.Id == id)
            .Select(s => new DeveloperDto
            {
                id = s.Id,
                name = s.Name
            }).FirstOrDefaultAsync();
            if (developer == null)
            {
                return NotFound();
            }

            return Ok(developer);
        }

        /// <summary>
        /// Updates a developer based on its' id.
        /// </summary>
        /// <returns>Developer</returns>
        /// <response code="204">Developer updated, no response</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Developer not found</response>
        // PUT: api/Developers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutDeveloper(Guid id, UpdateDeveloperDto developerDto)
        {
            if (!_context.Developers.Any(a => a.Id == id))
            {
                return NotFound();
            }

            var developer = new Developer { Id = id, Name = developerDto.name };
            _context.Entry(developer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeveloperExists(id))
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
        /// Adds a new Developer to the store.
        /// </summary>
        /// <returns>Developer</returns>
        /// <response code="201">Returns the developer</response>
        /// <response code="400">Bad request</response>
        // POST: api/Developers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DeveloperDto>> PostDeveloper(CreateDeveloperDto developerDto)
        {
            if (_context.Developers == null)
            {
                return Problem("Entity set 'BackloggedDBContext.Developer'  is null.");
            }
            var developer = new Developer
            {
                Name = developerDto.name
            };
            _context.Developers.Add(developer);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDeveloper), new { id = developer.Id }, new DeveloperDto { id = developer.Id, name = developer.Name });
        }

        /// <summary>
        /// Deletes a developer from the store.
        /// </summary>
        /// <returns>Developer</returns>
        /// <response code="204">Developer deleted, no response</response>
        /// <response code="404">Developer not found</response>
        // DELETE: api/Developers/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteDeveloper(Guid id)
        {
            if (_context.Developers == null)
            {
                return NotFound();
            }
            var developer = await _context.Developers.FindAsync(id);
            if (developer == null)
            {
                return NotFound();
            }

            _context.Developers.Remove(developer);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        /// <summary>
        /// Gets the games for a developer.
        /// </summary>
        /// <returns>backlog</returns>
        /// <response code="200">Games</response>
        /// <response code="404">developer not found</response>
        // Get: api/Backlogs/uuid/Games
        [HttpGet("{id}/Games")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGames(Guid id, [FromQuery] PagingParams pagingParams)
        {
            if (_context.Publishers == null)
            {
                return NotFound();
            }

            var games = await PageListBuilder.CreatePagedListAsync(_context.Developers
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


        private bool DeveloperExists(Guid id)
        {
            return (_context.Developers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
