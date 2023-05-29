using System;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backlogged_api.Data;
using backlogged_api.Models;
using backlogged_api.DTO.Publisher;

namespace backlogged_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class PublishersController : ControllerBase
    {
        private readonly BackloggedDBContext _context;

        public PublishersController(BackloggedDBContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Gets all publishers.
        /// </summary>
        /// <returns>All publishers</returns>
        /// <response code="200">Returns the publishers correctly</response>
        // GET: api/Publishers
        [MapToApiVersion("1.0")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PublisherDto>>> GetAllPublishers()
        {
            if (_context.Publisher == null)
            {
                return NotFound();
            }
            var publishers = await _context.Publisher.Select(s => new PublisherDto
            {
                id = s.id,
                name = s.name,
            }).ToListAsync();

            return Ok(publishers);
        }

        /// <summary>
        /// Gets a publisher based on their id.
        /// </summary>
        /// <returns>Publisher</returns>
        /// <response code="200">Returns the publisher</response>
        /// <response code="404">Publisher not found</response>
        // GET: api/Publishers/4e78f04c-74b6-458f-9590-40b2194af61b
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PublisherDto>> GetPublisher(Guid id)
        {
            if (_context.Publisher == null)
            {
                return NotFound();
            }
            var publisher = await _context.Publisher.Where(w => w.id == id).Select(s => new PublisherDto
            {
                id = s.id,
                name = s.name,
            }).FirstOrDefaultAsync();
            if (publisher == null)
            {
                return NotFound();
            }

            return Ok(publisher);
        }

        /// <summary>
        /// Updates a publisher's details.
        /// </summary>
        /// <returns>Publisher</returns>
        /// <response code="204">Publisher updated, no content</response>
        /// <response code="404">Publisher not found</response>
        /// <response code="400">Bad request</response>
        // GET: api/Publishers/4e78f04c-74b6-458f-9590-40b2194af61b
        // PUT: api/Publishers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutPublisher(Guid id, UpdatePublisherDto publisherDto)
        {
            if (!_context.Publisher.Any(a => a.id == id))
            {
                return NotFound();
            }

            var publisher = new Publisher { name = publisherDto.name, id = id }; 

            _context.Entry(publisher).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PublisherExists(id))
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
        /// Adds a new Publisher to the store.
        /// </summary>
        /// <returns>Publisher</returns>
        /// <response code="201">Publisher created</response>
        /// <response code="400">Bad request</response>
        // GET: api/Publishers/4e78f04c-74b6-458f-9590-40b2194af61b
        // PUT: api/Publishers/5
        // POST: api/Publishers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PublisherDto>> PostPublisher(CreatePublisherDto publisherDto)
        {
            if (_context.Publisher == null)
            {
                return Problem("Entity set 'BackloggedDBContext.Publisher'  is null.");
            }
            var publisher = new Publisher
            {
                name = publisherDto.name,
            };
            _context.Publisher.Add(publisher);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPublisher), new { id = publisher.id }, new
            {
                id = publisher.id,
                name = publisher.name
            });
        }
        /// <summary>
        /// Deletes a Publisher from the store.
        /// </summary>
        /// <returns>Publisher</returns>
        /// <response code="204">Publisher deleted, no content</response>
        /// <response code="404">Publisher not found</response>
        // DELETE: api/Publishers/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePublisher(Guid id)
        {
            if (_context.Publisher == null)
            {
                return NotFound();
            }
            var publisher = await _context.Publisher.FindAsync(id);
            if (publisher == null)
            {
                return NotFound();
            }

            _context.Publisher.Remove(publisher);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PublisherExists(Guid id)
        {
            return (_context.Publisher?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
