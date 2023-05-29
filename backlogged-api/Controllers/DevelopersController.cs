using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backlogged_api.Data;
using backlogged_api.Models;
using backlogged_api.DTO.Developer;

namespace backlogged_api.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<ActionResult<IEnumerable<DeveloperDto>>> GetAllDevelopers()
        {
            if (_context.Developer == null)
            {
                return NotFound();
            }
            var developers = await _context.Developer.Select(s => new DeveloperDto
            {
                id = s.id,
                name = s.name
            }).ToListAsync();
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
            if (_context.Developer == null)
            {
                return NotFound();
            }
            var developer = await _context.Developer
            .Where(w => w.id == id)
            .Select(s => new DeveloperDto
            {
                id = s.id,
                name = s.name
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
            if (!_context.Developer.Any(a => a.id == id))
            {
                return NotFound();
            }

            var developer = new Developer { id = id, name = developerDto.name };
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
            if (_context.Developer == null)
            {
                return Problem("Entity set 'BackloggedDBContext.Developer'  is null.");
            }
            var developer = new Developer
            {
                name = developerDto.name
            };
            _context.Developer.Add(developer);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDeveloper), new { id = developer.id }, new DeveloperDto { id = developer.id, name = developer.name });
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
            if (_context.Developer == null)
            {
                return NotFound();
            }
            var developer = await _context.Developer.FindAsync(id);
            if (developer == null)
            {
                return NotFound();
            }

            _context.Developer.Remove(developer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DeveloperExists(Guid id)
        {
            return (_context.Developer?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
