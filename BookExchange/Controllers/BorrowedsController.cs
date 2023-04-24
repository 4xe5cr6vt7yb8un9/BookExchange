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
    public class BorrowedsController : Controller
    {
        private readonly BookExchangeContext _context;

        public BorrowedsController(BookExchangeContext context)
        {
            _context = context;
        }

        // GET: Borroweds
        public async Task<IActionResult> Index()
        {
              return _context.Borrowed != null ? 
                          View(await _context.Borrowed.ToListAsync()) :
                          Problem("Entity set 'BookExchangeContext.Borrowed'  is null.");
        }

        // GET: Borroweds/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Borrowed == null)
            {
                return NotFound();
            }

            var borrowed = await _context.Borrowed
                .FirstOrDefaultAsync(m => m.Id == id);
            if (borrowed == null)
            {
                return NotFound();
            }

            return View(borrowed);
        }

        // GET: Borroweds/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Borroweds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ISBN")] Borrowed borrowed)
        {
            if (ModelState.IsValid)
            {
                borrowed.Id = Guid.NewGuid();
                _context.Add(borrowed);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(borrowed);
        }

        // GET: Borroweds/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Borrowed == null)
            {
                return NotFound();
            }

            var borrowed = await _context.Borrowed.FindAsync(id);
            if (borrowed == null)
            {
                return NotFound();
            }
            return View(borrowed);
        }

        // POST: Borroweds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,ISBN")] Borrowed borrowed)
        {
            if (id != borrowed.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(borrowed);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BorrowedExists(borrowed.Id))
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
            return View(borrowed);
        }

        // GET: Borroweds/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Borrowed == null)
            {
                return NotFound();
            }

            var borrowed = await _context.Borrowed
                .FirstOrDefaultAsync(m => m.Id == id);
            if (borrowed == null)
            {
                return NotFound();
            }

            return View(borrowed);
        }

        // POST: Borroweds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Borrowed == null)
            {
                return Problem("Entity set 'BookExchangeContext.Borrowed'  is null.");
            }
            var borrowed = await _context.Borrowed.FindAsync(id);
            if (borrowed != null)
            {
                _context.Borrowed.Remove(borrowed);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BorrowedExists(Guid id)
        {
          return (_context.Borrowed?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
