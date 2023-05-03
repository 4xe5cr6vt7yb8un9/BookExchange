using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookExchange.Data;
using BookExchange.Models;

namespace BookExchange.Controllers
{
    public class RentsController : Controller
    {
        private readonly BookExchangeContext _context;

        public RentsController(BookExchangeContext context)
        {
            _context = context;
        }

        // GET: Rents
        public async Task<IActionResult> Index()
        {
              return _context.Rents != null ? 
                          View(await _context.Rents.ToListAsync()) :
                          Problem("Entity set 'BookExchangeContext.Rents'  is null.");
        }

        // GET: Rents/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Rents == null)
            {
                return NotFound();
            }

            var rents = await _context.Rents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rents == null)
            {
                return NotFound();
            }

            return View(rents);
        }

        // GET: Rents/Create
        public IActionResult RentBook()
        {
            return View();
        }

        // GET: Rents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RenterName,RenterEmail,RentedFrom,ISBN,RentDate")] Rents rents)
        {
            if (ModelState.IsValid)
            {
                rents.Id = Guid.NewGuid();
                _context.Add(rents);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rents);
        }

        // GET: Rents/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Rents == null)
            {
                return NotFound();
            }

            var rents = await _context.Rents.FindAsync(id);
            if (rents == null)
            {
                return NotFound();
            }
            return View(rents);
        }

        // POST: Rents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,RenterName,RenterEmail,ISBN,RentDate")] Rents rents)
        {
            if (id != rents.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rents);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentsExists(rents.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(rents);
        }

        // GET: Rents/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Rents == null)
            {
                return NotFound();
            }

            var rents = await _context.Rents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rents == null)
            {
                return NotFound();
            }

            return View(rents);
        }

        // POST: Rents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Rents == null)
            {
                return Problem("Entity set 'BookExchangeContext.Rents'  is null.");
            }
            var rents = await _context.Rents.FindAsync(id);
            if (rents != null)
            {
                _context.Rents.Remove(rents);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentsExists(Guid id)
        {
          return (_context.Rents?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
