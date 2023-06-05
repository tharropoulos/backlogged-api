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
using backlogged_api.Helpers;

namespace backlogged_api.Controllers
{
    [Route("api/v1/[controller]")]
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
        public async Task<ActionResult<IEnumerable<PublisherDto>>> GetAllPublishers([FromQuery] PagingParams pagingParams)
        {
            if (_context.Publishers == null)
            {
                return NotFound();
            }
            var publishers = await PageListBuilder.CreatePagedListAsync(_context.Publishers.Select(s => new PublisherDto
            {
                id = s.Id,
                name = s.Name
            }), m => m.name, pagingParams.PageNumber, pagingParams.PageSize);
            var metadata = new
            {
                publishers.TotalCount,
                publishers.PageSize,
                publishers.CurrentPage,
                publishers.TotalPages,
                publishers.HasNext,
                publishers.HasPrevious
            };

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
            if (_context.Publishers == null)
            {
                return NotFound();
            }
            var publisher = await _context.Publishers.Where(w => w.Id == id).Select(s => new PublisherDto
            {
                id = s.Id,
                name = s.Name,
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
        // PUT: api/Publishers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutPublisher(Guid id, UpdatePublisherDto publisherDto)
        {
            if (!_context.Publishers.Any(a => a.Id == id))
            {
                return NotFound();
            }

            var publisher = new Publisher { Name = publisherDto.name, Id = id };

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
            if (_context.Publishers == null)
            {
                return Problem("Entity set 'BackloggedDBContext.Publisher'  is null.");
            }
            var publisher = new Publisher
            {
                Name = publisherDto.name,
            };
            _context.Publishers.Add(publisher);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPublisher), new { id = publisher.Id }, new
            {
                id = publisher.Id,
                name = publisher.Name
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
            if (_context.Publishers == null)
            {
                return NotFound();
            }
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher == null)
            {
                return NotFound();
            }

            _context.Publishers.Remove(publisher);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PublisherExists(Guid id)
        {
            return (_context.Publishers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
