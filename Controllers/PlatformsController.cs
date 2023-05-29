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

namespace backlogged_api.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<ActionResult<IEnumerable<PlatformDto>>> GetAllPlatforms()
        {
            if (_context.Platform == null)
            {
                return NotFound();
            }
            var platforms = await _context.Platform.Select(p => new PlatformDto
            {
                id = p.id,
                name = p.name
            }).ToListAsync();
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
            if (_context.Platform == null)
            {
                return NotFound();
            }
            var platform = await _context.Platform
            .Where(w => w.id == id)
            .Select(s => new PlatformDto
            {
                id = s.id,
                name = s.name
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

            if (!_context.Platform.Any(a => a.id == id))
            {
                return NotFound();
            }

            var platform = new Platform
            {
                id = id,
                name = platformDto.name
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
            if (_context.Platform == null)
            {
                return Problem("Entity set 'BackloggedDBContext.Platform'  is null.");
            }
            var platform = new Platform
            {
                name = platformDto.name
            };
            _context.Platform.Add(platform);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPlatform), new { id = platform.id }, new PlatformDto { id = platform.id, name = platform.name });
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
            if (_context.Platform == null)
            {
                return NotFound();
            }
            var platform = await _context.Platform.FindAsync(id);
            if (platform == null)
            {
                return NotFound();
            }

            _context.Platform.Remove(platform);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlatformExists(Guid id)
        {
            return (_context.Platform?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
