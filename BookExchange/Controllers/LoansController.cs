using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookExchange.Data;
using BookExchange.Models;
using BookExchange.Controllers;

namespace BookExchange.Controllers
{
    public class LoansController : Controller
    {
        private readonly BookExchangeContext _context;

        public LoansController(BookExchangeContext context)
        {
            _context = context;
        }

        // GET: Loans
        public async Task<IActionResult> Index()
        {
              return _context.Loans != null ? 
                          View(await _context.Loans.ToListAsync()) :
                          Problem("Entity set 'BookExchangeContext.Loans'  is null.");
        }

        // GET: Loans/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Loans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LoanerName,LoanerEmail,ISBN,LoanDate")] Loans loans)
        {
            if (ModelState.IsValid)
            {
                loans.Id = Guid.NewGuid();
                loans.LoanDate = DateTime.Now;
                _context.Add(loans);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(loans);
        }

        public IActionResult Rent()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RentCon(Guid? id, [Bind("Id,RenterName,RenterEmail")] Rents rent)
        {
            Console.WriteLine(id);
            if (id == null || _context.Loans == null)
            {
                return NotFound();
            }

            var loans = await _context.Loans.FindAsync(id);
            if (loans == null)
            {
                return NotFound();
            }

            Rents rents = (new Rents
            {
                Id = Guid.NewGuid(),
                RenterName = rent.RenterName,
                RenterEmail = rent.RenterEmail,
                RentedFrom = loans.LoanerName,
                RentDate = DateTime.Now,
                ISBN = loans.ISBN
            });

            return RedirectToAction("Create", "Rents", rents);
        }

        // GET: Loans/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Loans == null)
            {
                return NotFound();
            }

            var loans = await _context.Loans
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loans == null)
            {
                return NotFound();
            }

            return View(loans);
        }

        // POST: Loans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Loans == null)
            {
                return Problem("Entity set 'BookExchangeContext.Loans'  is null.");
            }
            var loans = await _context.Loans.FindAsync(id);
            if (loans != null)
            {
                _context.Loans.Remove(loans);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoansExists(Guid id)
        {
          return (_context.Loans?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
