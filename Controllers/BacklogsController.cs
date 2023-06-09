using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backlogged_api.Data;
using backlogged_api.Models;
using backlogged_api.DTO.Backlog;
using backlogged_api.DTO.Developer;
using backlogged_api.DTO.Genre;
using backlogged_api.DTO.Platform;
using backlogged_api.DTO.Franchise;
using backlogged_api.DTO.Review;
using backlogged_api.Helpers;
using Newtonsoft.Json;
using System.Linq.Expressions;
using backlogged_api.DTO.Game;
using Microsoft.AspNetCore.Authorization;

namespace backlogged_api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class BacklogController : ControllerBase
    {
        private readonly BackloggedDBContext _context;

        public BacklogController(BackloggedDBContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Gets all backlogs.
        /// </summary>
        /// <returns>All backlogs</returns>
        /// <response code="200">Returns the backlogs correctly</response>
        // GET: api/Backlogs
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BacklogDto>>> GetAllBacklogs([FromQuery] PagingParams pagingParams)
        {
            if (_context.Backlogs == null)
            {
                return NotFound();
            }

            var backlogs = await PageListBuilder.CreatePagedListAsync(_context.Backlogs, m => m.IsVisible, pagingParams.PageNumber, pagingParams.PageSize);

            var metadata = new
            {
                backlogs.TotalCount,
                backlogs.PageSize,
                backlogs.CurrentPage,
                backlogs.TotalPages,
                backlogs.HasNext,
                backlogs.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            var backlogsDto = backlogs.Select(s => new BacklogDto
            {
                Id = s.Id,
                UserId = s.UserId,
                IsVisible = s.IsVisible
            });
            return Ok(backlogsDto);
        }

        /// <summary>
        /// Gets a backlog based on its' id.
        /// </summary>
        /// <returns>backlog</returns>
        /// <response code="200">Returns the backlog</response>
        /// <response code="404">Backlog not found</response>
        // GET: api/Backlogs/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BacklogDto>> GetBacklog(Guid id)
        {
            if (_context.Backlogs == null)
            {
                return NotFound();
            }

            var backlog = await _context.Backlogs
            .Where(w => w.Id == id)
            .Select(s => new BacklogDto
            {
                Id = s.Id,
                UserId = s.UserId,
                IsVisible = s.IsVisible

            }).FirstOrDefaultAsync();

            if (backlog == null)
            {
                return NotFound();
            }

            return Ok(backlog);
        }


        /// <summary>
        /// Updates a backlog visibility based on its' id.
        /// </summary>
        /// <returns>backlog</returns>
        /// <response code="204">backlog updated, no response</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">backlog not found</response>
        // PUT: api/Backlogs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPatch("{id}/Visibility")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PatchBacklogVisibility(Guid id, UpdateBacklogDto backlogDto)
        {

            var backlogToUpdate = await _context.Backlogs
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (backlogToUpdate == null)
            {
                return NotFound();
            }

            backlogToUpdate.IsVisible = backlogDto.IsVisible;

            _context.Entry(backlogToUpdate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BacklogExists(id))
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
        /// Gets the games for a backlog.
        /// </summary>
        /// <returns>backlog</returns>
        /// <response code="200">Games</response>
        /// <response code="404">Backlog not found</response>
        // Get: api/Backlogs/uuid/Games
        [HttpGet("{id}/Games")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGames(Guid id, [FromQuery] PagingParams pagingParams)
        {
            if (_context.Backlogs == null)
            {
                return NotFound();
            }

            var games = await PageListBuilder.CreatePagedListAsync(_context.Backlogs
                .Include(i => i.Games)
                .Where(w => w.Id == id)
                .SelectMany(s => s.Games), m => m.Rating, pagingParams.PageNumber, pagingParams.PageSize);

            var gameDtos = games.Select(s => new GameDto
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
            });

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

            return Ok(gameDtos);
        }
        /// <summary>
        /// Updates a backlog's games list. Use "add" or "remove" operation to update the list.
        /// </summary>
        /// <returns>game</returns>
        /// <response code="204">backlog updated, no response</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">game not found</response>
        // PUT: api/Games/5
        [HttpPatch("{id}/Games")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutDevelopers(Guid id, UpdateBacklogGamesDto updateBacklogDto)
        {
            if (_context.Backlogs == null)
                return NotFound();

            var backlog = await _context.Backlogs
                .Include(i => i.Games)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (backlog == null)
                return NotFound("backlog not Found");


            // if a backlog already has a game, return bad request
            if (backlog.Games.Where(w => updateBacklogDto.GameIds.Contains(w.Id)).Any())
                return BadRequest("Game already exists in backlog");

            if (updateBacklogDto.Operation == "add")
            {
                var games = await _context.Games.Where(w => updateBacklogDto.GameIds.Contains(w.Id)).ToListAsync();
                foreach (var game in games)
                {
                    backlog.Games.Add(game);
                }
                _context.Entry(backlog).State = EntityState.Modified;
            }
            else if (updateBacklogDto.Operation == "remove")
            {
                var games = await _context.Games.Where(w => updateBacklogDto.GameIds.Contains(w.Id)).ToListAsync();
                foreach (var game in games)
                {
                    backlog.Games.Remove(game);
                }
                _context.Entry(backlog).State = EntityState.Modified;
            }
            else
            {
                return BadRequest("Invalid Operation");
            }


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!BacklogExists(id))
            {
                return NotFound();
            }

            return NoContent();

        }

        private bool BacklogExists(Guid id)
        {
            return (_context.Backlogs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
