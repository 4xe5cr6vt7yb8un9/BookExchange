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
    public class ExchangeUsersController : Controller
    {
        private readonly BookExchangeContext _context;

        public ExchangeUsersController(BookExchangeContext context)
        {
            _context = context;
        }

        // GET: ExchangeUsers
        public async Task<IActionResult> Index()
        {
              return _context.ExchangeUser != null ? 
                          View(await _context.ExchangeUser.ToListAsync()) :
                          Problem("Entity set 'BookExchangeContext.ExchangeUser'  is null.");
        }

        // GET: ExchangeUsers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.ExchangeUser == null)
            {
                return NotFound();
            }

            var exchangeUser = await _context.ExchangeUser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exchangeUser == null)
            {
                return NotFound();
            }

            return View(exchangeUser);
        }

        // GET: ExchangeUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ExchangeUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email")] ExchangeUser exchangeUser)
        {
            if (ModelState.IsValid)
            {
                exchangeUser.Id = Guid.NewGuid();
                _context.Add(exchangeUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(exchangeUser);
        }

        // GET: ExchangeUsers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.ExchangeUser == null)
            {
                return NotFound();
            }

            var exchangeUser = await _context.ExchangeUser.FindAsync(id);
            if (exchangeUser == null)
            {
                return NotFound();
            }
            return View(exchangeUser);
        }

        // POST: ExchangeUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Email")] ExchangeUser exchangeUser)
        {
            if (id != exchangeUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exchangeUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExchangeUserExists(exchangeUser.Id))
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
            return View(exchangeUser);
        }

        // GET: ExchangeUsers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.ExchangeUser == null)
            {
                return NotFound();
            }

            var exchangeUser = await _context.ExchangeUser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exchangeUser == null)
            {
                return NotFound();
            }

            return View(exchangeUser);
        }

        // POST: ExchangeUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.ExchangeUser == null)
            {
                return Problem("Entity set 'BookExchangeContext.ExchangeUser'  is null.");
            }
            var exchangeUser = await _context.ExchangeUser.FindAsync(id);
            if (exchangeUser != null)
            {
                _context.ExchangeUser.Remove(exchangeUser);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExchangeUserExists(Guid id)
        {
          return (_context.ExchangeUser?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
