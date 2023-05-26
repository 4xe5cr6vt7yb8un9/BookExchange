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
        [Route("Loans/{pageNumber?}/{seachString?}")]
        public async Task<IActionResult> Page(string searchString, string currentFilter, int? pageNumber)
        {

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var loans = from loan in _context.Loans
                        select loan;

            if (!String.IsNullOrEmpty(searchString))
            {
                loans = loans.Where(s => s.ISBN.Contains(searchString));
            }

            loans = loans.OrderByDescending(loans => loans.LoanDate);

            int pageSize = 11;
            return View(await PaginatedList<Loans>.CreateAsync(loans.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Loans/Create
        [Route("Loans/Create/{isbn?}")]
        public IActionResult Create(String? isbn)
        {
            return View();
        }

        // POST: Loans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Loans/Create/{isbn?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LoanerName,LoanerEmail,ISBN,LoanDate")] Loans loans)
        {
            if (ModelState.IsValid)
            {
                loans.Id = Guid.NewGuid();
                loans.LoanDate = DateTime.Now;
                _context.Add(loans);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Page));
            }
            return View(loans);
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
            return RedirectToAction(nameof(Page));
        }

        private bool LoansExists(Guid id)
        {
            return (_context.Loans?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
