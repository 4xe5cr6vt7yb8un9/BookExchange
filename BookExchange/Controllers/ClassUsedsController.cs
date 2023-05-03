using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookExchange.Data;
using BookExchange.Models;

namespace BookExchange.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassUsedsController : ControllerBase
    {
        private readonly BookExchangeContext _context;

        public ClassUsedsController(BookExchangeContext context)
        {
            _context = context;
        }

        // GET: api/ClassUseds
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassUsed>>> GetClassUsed()
        {
          if (_context.ClassUsed == null)
          {
              return NotFound();
          }
            return await _context.ClassUsed.ToListAsync();
        }

        // GET: api/ClassUseds/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClassUsed>> GetClassUsed(Guid id)
        {
          if (_context.ClassUsed == null)
          {
              return NotFound();
          }
            var classUsed = await _context.ClassUsed.FindAsync(id);

            if (classUsed == null)
            {
                return NotFound();
            }

            return classUsed;
        }

        // PUT: api/ClassUseds/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClassUsed(Guid id, ClassUsed classUsed)
        {
            if (id != classUsed.ClassUsedID)
            {
                return BadRequest();
            }

            _context.Entry(classUsed).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassUsedExists(id))
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

        // POST: api/ClassUseds
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ClassUsed>> PostClassUsed(ClassUsed classUsed)
        {
          if (_context.ClassUsed == null)
          {
              return Problem("Entity set 'BookExchangeContext.ClassUsed'  is null.");
          }
            _context.ClassUsed.Add(classUsed);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClassUsed", new { id = classUsed.ClassUsedID }, classUsed);
        }

        // DELETE: api/ClassUseds/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClassUsed(Guid id)
        {
            if (_context.ClassUsed == null)
            {
                return NotFound();
            }
            var classUsed = await _context.ClassUsed.FindAsync(id);
            if (classUsed == null)
            {
                return NotFound();
            }

            _context.ClassUsed.Remove(classUsed);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClassUsedExists(Guid id)
        {
            return (_context.ClassUsed?.Any(e => e.ClassUsedID == id)).GetValueOrDefault();
        }
    }
}
