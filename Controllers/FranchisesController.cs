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

namespace backlogged_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class FranchisesController : ControllerBase
    {
        private readonly BackloggedDBContext _context;

        public FranchisesController(BackloggedDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all franchises.
        /// </summary>
        /// <returns>All franchises</returns>
        /// <response code="200">Returns the franchises correctly</response>
        // GET: api/Franchises
        [MapToApiVersion("1.0")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<FranchiseDto>>> GetAllFranchises()
        {
            if (_context.Franchises == null)
            {
                return NotFound();
            }
            var franchises = await _context.Franchises.Select(s => new FranchiseDto
            {
                id = s.id,
                name = s.name
            }).ToListAsync();

            return Ok(franchises);
        }

        /// <summary>
        /// Gets a franchise based on its' id.
        /// </summary>
        /// <returns>Franchise</returns>
        /// <response code="200">Returns the franchise</response>
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
            .Where(w => w.id == id)
            .Select(s => new FranchiseDto
            {
                id = s.id,
                name = s.name
            }).FirstOrDefaultAsync();

            if (franchise == null)
            {
                return NotFound();
            }

            return franchise;
        }

        /// <summary>
        /// Updates a franchise's details.
        /// </summary>
        /// <response code="204">Publisher updated, no content</response>
        /// <response code="404">Publisher not found</response>
        /// <response code="400">Bad request</response>
        // PUT: api/Franchises/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutFranchise(Guid id, UpdateFranchiseDto franchiseDto)
        {
            if (!_context.Franchises.Any(a => a.id == id))
            {
                return NotFound();
            }

            var franchise = new Franchise { name = franchiseDto.name, id = id };
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
        /// <response code="201">Publisher created</response>
        /// <response code="400">Bad request</response>
        // POST: api/Publishers
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
                name = franchiseDto.name
            };
            _context.Franchises.Add(franchise);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFranchise), new { id = franchise.id }, new FranchiseDto { id = franchise.id, name = franchise.name });
        }
        /// <summary>
        /// Deletes a Publisher from the store.
        /// </summary>
        /// <returns>Publisher</returns>
        /// <response code="204">Publisher deleted, no content</response>
        /// <response code="404">Publisher not found</response>

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

        private bool FranchiseExists(Guid id)
        {
            return (_context.Franchises?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
