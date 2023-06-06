using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backlogged_api.Data;
using backlogged_api.Models;
using backlogged_api.DTO.Game;
using backlogged_api.DTO.Developer;
using backlogged_api.DTO.Genre;
using backlogged_api.DTO.Platform;
using backlogged_api.DTO.Franchise;
using backlogged_api.DTO.Review;
using backlogged_api.Helpers;
using Newtonsoft.Json;
using System.Linq.Expressions;
using backlogged_api.DTO.Publisher;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace backlogged_api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class GamesController : ControllerBase
    {
        private readonly BackloggedDBContext _context;

        public GamesController(BackloggedDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all games.
        /// </summary>
        /// <returns>All games</returns>
        /// <response code="200">Returns the games correctly</response>
        // GET: api/Games
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetAllGames([FromQuery] PagingParams pagingParams)
        {
            if (_context.Games == null)
            {
                return NotFound();
            }

            var games = await PageListBuilder.CreatePagedListAsync(_context.Games, m => m.Title, pagingParams.PageNumber, pagingParams.PageSize);

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

            var gamesDto = games.Select(s => new GameDto
            {
                Id = s.Id,
                Rating = s.Rating,
                BackgoundImageUrl = s.BackgroundImageUrl,
                CoverImageUrl = s.CoverImageUrl,
                FranchiseId = s.FranchiseId,
                PublisherId = s.PublisherId,
                Title = s.Title,
                Description = s.Description,
                ReleaseDate = s.ReleaseDate,
            });
            return Ok(gamesDto);
        }

        /// <summary>
        /// Gets a game based on its' id.
        /// </summary>
        /// <returns>game</returns>
        /// <response code="200">Returns the game</response>
        /// <response code="404">game not found</response>
        // GET: api/Games/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GameDto>> GetGame(Guid id)
        {
            if (_context.Games == null)
            {
                return NotFound("No games found.");
            }

            var game = await _context.Games
            .Where(w => w.Id == id)
            .Select(s => new GameDto
            {
                Id = s.Id,
                Rating = s.Rating,
                BackgoundImageUrl = s.BackgroundImageUrl,
                CoverImageUrl = s.CoverImageUrl,
                FranchiseId = s.FranchiseId,
                PublisherId = s.PublisherId,
                Title = s.Title,
                Description = s.Description,
                ReleaseDate = s.ReleaseDate,

            }).FirstOrDefaultAsync();

            if (game == null)
            {
                return NotFound("No game found.");
            }

            return Ok(game);
        }

        [HttpPost("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<GameDto>>> SearchGames([FromBody] GameSearchParams searchParams, [FromQuery] PagingParams pagingParams)
        {
            if (_context.Games == null)
            {
                return NotFound();
            }

            Expression<Func<Game, bool>> titleExpression = w => searchParams.Title == null || w.Title.Contains(searchParams.Title);
            Expression<Func<Game, bool>> genreExpression = w => searchParams.GenreIds == null || (w.Genres).Any(a => searchParams.GenreIds.Contains(a.Id));
            Expression<Func<Game, bool>> platformExpression = w => searchParams.PlatformIds == null || (w.Platforms).Any(a => searchParams.PlatformIds.Contains(a.Id));
            Expression<Func<Game, bool>> developerExpression = w => searchParams.DeveloperIds == null || (w.Developers).Any(a => searchParams.DeveloperIds.Contains(a.Id));
            Expression<Func<Game, bool>> publisherExpression = w => searchParams.PublisherIds == null || w.PublisherId.HasValue && searchParams.PublisherIds.Contains(w.PublisherId.Value);
            Expression<Func<Game, bool>> franchiseExpression = w => searchParams.FranchiseIds == null || w.FranchiseId.HasValue && searchParams.FranchiseIds.Contains(w.FranchiseId.Value);

            var games = await PageListBuilder.CreatePagedListAsync(_context.Games
                        .Where(titleExpression)
                        .Where(genreExpression)
                        .Where(platformExpression)
                        .Where(developerExpression)
                        .Where(publisherExpression)
                        .Where(franchiseExpression)
                        .Select(s => new GameDto
                        {
                            Id = s.Id,
                            Rating = s.Rating,
                            BackgoundImageUrl = s.BackgroundImageUrl,
                            CoverImageUrl = s.CoverImageUrl,
                            FranchiseId = s.FranchiseId,
                            PublisherId = s.PublisherId,
                            Title = s.Title,
                            Description = s.Description,
                            ReleaseDate = s.ReleaseDate,
                        }), o => o.Title, OrderDirection.Ascending, pagingParams.PageNumber, pagingParams.PageSize);

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

        /// <summary>
        /// Updates a game based on its' id.
        /// </summary>
        /// <returns>game</returns>
        /// <response code="204">game updated, no response</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">game not found</response>
        // PUT: api/Games/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutGame(Guid id, UpdateGameDto gameDto)
        {

            var gameToUpdate = await _context.Games
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (gameToUpdate == null)
            {
                return NotFound("No game found.");
            }

            gameToUpdate.FranchiseId = gameDto.FranchiseId;
            gameToUpdate.PublisherId = gameDto.PublisherId;
            gameToUpdate.Rating = gameDto.Rating;
            gameToUpdate.Description = gameDto.Description;
            gameToUpdate.CoverImageUrl = gameDto.CoverImageUrl;
            gameToUpdate.BackgroundImageUrl = gameDto.BackgoundImageUrl;

            _context.Entry(gameToUpdate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(id))
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
        /// Adds a new game to the store.
        /// </summary>
        /// <returns>game</returns>
        /// <response code="201">Returns the game</response>
        /// <response code="400">Bad request</response>
        // POST: api/Games
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GameDto>> Postgame(CreateGameDto gameDto)
        {
            if (_context.Games == null)
            {
                return Problem("Entity set 'BackloggedDBContext.Game'  is null.");
            }
            if (!_context.Publishers.Any(p => p.Id == gameDto.PublisherId))
            {
                return BadRequest("Publisher does not exist.");
            }

            if (!_context.Franchises.Any(f => f.Id == gameDto.FranchiseId))
            {
                return BadRequest("Franchise does not exist.");
            }
            var game = new Game
            {
                Title = gameDto.Title,
                Rating = gameDto.Rating,
                Description = gameDto.Description,
                BackgroundImageUrl = gameDto.BackgoundImageUrl,
                CoverImageUrl = gameDto.CoverImageUrl,
                FranchiseId = gameDto.FranchiseId,
                PublisherId = gameDto.PublisherId,
                // could be redudant, but just in case
                ReleaseDate = gameDto.ReleaseDate != DateTime.MinValue ? gameDto.ReleaseDate : DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)
            };
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetGame), new { id = game.Id }, new GameDto
            {
                Id = game.Id,
                Title = game.Title,
                Rating = game.Rating,
                Description = game.Description,
                ReleaseDate = game.ReleaseDate,
                PublisherId = game.PublisherId,
                FranchiseId = game.FranchiseId,
                CoverImageUrl = game.CoverImageUrl,
                BackgoundImageUrl = game.BackgroundImageUrl
            });
        }

        /// <summary>
        /// Deletes a game from the store.
        /// </summary>
        /// <returns>game</returns>
        /// <response code="204">game deleted, no response</response>
        /// <response code="404">game not found</response>
        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteGame(Guid id)
        {
            if (_context.Games == null)
            {
                return NotFound();
            }
            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Gets the platforms for a game.
        /// </summary>
        /// <returns>game</returns>
        /// <response code="200">Developers</response>
        /// <response code="404">Game not found</response>
        // Get: api/Games/uuid/Developers
        [HttpGet("{id}/Developers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<DeveloperDto>>> GetDevelopers(Guid id, [FromQuery] PagingParams pagingParams)
        {
            if (_context.Games == null)
            {
                return NotFound();
            }
            if (!_context.Games.Any(a => a.Id == id))
            {
                return NotFound("Game not found.");
            }

            var developers = await PageListBuilder.CreatePagedListAsync(_context.Games
                .Include(i => i.Developers)
                .Where(w => w.Id == id)
                .SelectMany(s => s.Developers), m => m.Name, pagingParams.PageNumber, pagingParams.PageSize);

            var developerDtos = developers.Select(s => new DeveloperDto
            {
                id = s.Id,
                name = s.Name,
            });

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

            return Ok(developerDtos);
        }
        /// <summary>
        /// Updates a game's developers list.
        /// </summary>
        /// <returns>game</returns>
        /// <response code="204">game updated, no response</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">game not found</response>
        // PUT: api/Games/5
        [HttpPut("{id}/Developers")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutDevelopers(Guid id, DeveloperIdList developersId)
        {
            if (_context.Games == null)
                return NotFound();

            var game = await _context.Games
                .Include(i => i.Developers)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (game == null)
                return NotFound("Game not Found");

            game.Developers = new List<Developer>();

            if ((developersId?.DeveloperIds).GroupBy(g => g).Any(g => g.Count() > 1))
                return BadRequest("Duplicate developer ids.");



            if ((developersId?.DeveloperIds).Count() == 0)
                _context.Entry(game).State = EntityState.Modified;

            else
            {
                // Declare devIds here to avoid errors in query
                var devIds = developersId?.DeveloperIds;

                var developers = await _context.Developers.Where(w => devIds.Contains(w.Id)).ToListAsync();


                if (developers.Count != devIds.Count() || developers.Count == 0)
                    return BadRequest("Developer does not exist.");

                foreach (var dev in developers)
                    game.Developers.Add(dev);

                _context.Entry(game).State = EntityState.Modified;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!GameExists(id))
            {
                return NotFound();
            }

            return NoContent();

        }

        /// <summary>
        /// Gets the genres for a game.
        /// </summary>
        /// <returns>game</returns>
        /// <response code="200">Genres</response>
        /// <response code="404">Game not found</response>
        // Get: api/Games/uuid/Genres
        [HttpGet("{id}/Genres")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<GenreDto>>> GetGenres(Guid id, [FromQuery] PagingParams pagingParams)
        {
            if (_context.Games == null)
            {
                return NotFound();
            }
            if (!GameExists(id))
            {
                return NotFound("Game not found.");
            }
            var genres = await PageListBuilder.CreatePagedListAsync(_context.Games
                .Include(i => i.Genres)
                .Where(w => w.Id == id)
                .SelectMany(s => s.Genres), m => m.Name, pagingParams.PageNumber, pagingParams.PageSize);

            var genreDtos = genres.Select(s => new DeveloperDto
            {
                id = s.Id,
                name = s.Name,
            });

            var metadata = new
            {
                genres.TotalCount,
                genres.PageSize,
                genres.CurrentPage,
                genres.TotalPages,
                genres.HasNext,
                genres.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(genreDtos);
        }
        /// <summary>
        /// Updates a game's genres list.
        /// </summary>
        /// <returns>game</returns>
        /// <response code="204">game updated, no response</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">game not found</response>
        // PUT: api/Games/5
        [HttpPut("{id}/Genres")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutGenres(Guid id, GenreIdList genreIds)
        {
            if (_context.Games == null)
                return NotFound();

            var game = await _context.Games
                .Include(i => i.Genres)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (game == null)
                return NotFound("Game not Found");

            game.Genres = new List<Genre>();

            if ((genreIds.GenreIds).GroupBy(g => g).Any(g => g.Count() > 1))
                return BadRequest("Duplicate genre ids.");



            if ((genreIds.GenreIds).Count() == 0)
                _context.Entry(game).State = EntityState.Modified;

            else
            {
                // TODO: Scuffed, many warnings for possible null references
                var genres = await _context.Genres.Where(w => (genreIds.GenreIds).Contains(w.Id)).ToListAsync();


                if (genres.Count() != (genreIds.GenreIds).Count() || (genreIds.GenreIds).Count() == 0)
                    return BadRequest("Genre does not exist.");

                foreach (var plat in genres)
                    game.Genres.Add(plat);

                _context.Entry(game).State = EntityState.Modified;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!GameExists(id))
            {
                return NotFound();
            }

            return NoContent();

        }

        /// <summary>
        /// Gets the platforms for a game.
        /// </summary>
        /// <returns>game</returns>
        /// <response code="200">Platforms</response>
        /// <response code="404">Game not found</response>
        // Get: api/Games/uuid/Platforms
        [HttpGet("{id}/Platforms")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<PlatformDto>>> GetPlatforms(Guid id, [FromQuery] PagingParams pagingParams)
        {
            if (_context.Games == null)
            {
                return NotFound();
            }

            if (!GameExists(id))
            {
                return NotFound("Game not Found");
            }

            var platforms = await PageListBuilder.CreatePagedListAsync(_context.Games
                .Include(i => i.Platforms)
                .Where(w => w.Id == id)
                .SelectMany(s => (s.Platforms)), m => m.Name, pagingParams.PageNumber, pagingParams.PageSize);

            var platformDtos = platforms.Select(s => new DeveloperDto
            {
                id = s.Id,
                name = s.Name,
            });

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

            return Ok(platformDtos);
        }
        /// <summary>
        /// Updates a game's platforms list.
        /// </summary>
        /// <returns>game</returns>
        /// <response code="204">game updated, no response</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">game not found</response>
        // PUT: api/Games/5
        [HttpPut("{id}/Platforms")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutPlatforms(Guid id, PlatformIdList platformIds)
        {
            if (_context.Games == null)
                return NotFound();

            var game = await _context.Games
                .Include(i => i.Platforms)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (game == null)
                return NotFound("Game not Found");

            game.Platforms = new List<Platform>();

            if ((platformIds.PlatformIds).GroupBy(g => g).Any(g => g.Count() > 1))
                return BadRequest("Duplicate platform ids.");



            if ((platformIds.PlatformIds).Count() == 0)
                _context.Entry(game).State = EntityState.Modified;

            else
            {
                // TODO: Scuffed, many warnings for possible null references
                var platforms = await _context.Platforms.Where(w => (platformIds.PlatformIds).Contains(w.Id)).ToListAsync();


                if (platforms.Count != (platformIds.PlatformIds).Count() || (platformIds.PlatformIds).Count() == 0)
                    return BadRequest("Platform does not exist.");

                foreach (var plat in platforms)
                    game.Platforms.Add(plat);

                _context.Entry(game).State = EntityState.Modified;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!GameExists(id))
            {
                return NotFound();
            }

            return NoContent();

        }
        /// <summary>
        /// Gets the franchise for a game.
        /// </summary>
        /// <returns>game</returns>
        /// <response code="200">Developers</response>
        /// <response code="404">Game not found</response>
        // Get: api/Games/uuid/Developers
        [HttpGet("{id}/Franchise")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FranchiseDto>> GetFranchise(Guid id)
        {
            if (_context.Games == null)
            {
                return NotFound();
            }
            if (!GameExists(id))
            {
                return NotFound("Game not Found");
            }

            var franchise = await _context.Games.Include(i => i.Franchise)
            .Where(w => w.Id == id)
            .Select(s => new FranchiseDto
            {
                id = s.FranchiseId.HasValue ? s.FranchiseId.Value : Guid.Empty,
                name = s.Franchise.Name
            }).FirstOrDefaultAsync();

            return Ok(franchise);


        }

        /// <summary>
        /// Gets the publisher for a game.
        /// </summary>
        /// <returns>game</returns>
        /// <response code="200">Publisher</response>
        /// <response code="404">Game not found</response>
        // Get: api/Games/uuid/Publisher
        [HttpGet("{id}/Publisher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FranchiseDto>> GetPublisher(Guid id)
        {
            if (_context.Games == null)
            {
                return NotFound();
            }
            if (!GameExists(id))
            {
                return NotFound("Game not Found");
            }

            var publisher = await _context.Games.Include(i => i.Publisher)
            .Where(w => w.Id == id)
            .Select(s => new PublisherDto
            {
                id = s.PublisherId.HasValue ? s.FranchiseId.Value : Guid.Empty,
                name = s.Publisher.Name
            }).FirstOrDefaultAsync();

            return Ok(publisher);


        }

        /// <summary>
        /// Gets the Reviews for a game.
        /// </summary>
        /// <returns>review</returns>
        /// <response code="200">Reviews</response>
        /// <response code="404">Game not found</response>
        // Get: api/Games/uuid/Developers
        [HttpGet("{id}/Reviews")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviews(Guid id, [FromQuery] PagingParams pagingParams)
        {
            if (_context.Games == null)
            {
                return NotFound();
            }

            if (!GameExists(id))
                return NotFound("Game not found.");

            var reviews = await PageListBuilder.CreatePagedListAsync(_context.Reviews
                .Include(i => i.Game)
                .Where(w => w.Game.Id == id)
                .Select(s => new ReviewDto
                {
                    AuthorId = s.AuthorId,
                    GameId = s.GameId,
                    Id = s.Id,
                    Rating = s.Rating,
                    Details = s.Details
                }), m => m.Id, pagingParams.PageNumber, pagingParams.PageSize);

            var metadata = new
            {
                reviews.TotalCount,
                reviews.PageSize,
                reviews.CurrentPage,
                reviews.TotalPages,
                reviews.HasNext,
                reviews.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(reviews);
        }
        private bool GameExists(Guid id)
        {
            return (_context.Games?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
