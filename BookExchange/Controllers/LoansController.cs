using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookExchange.Data;
using BookExchange.Models;

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
            // Redirects to page 1 when a search query is present
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            // Gathers book titles
            var books = _context.Book.Where(x => _context.Loans.Any(s => s.BookID == x.BookID));

            // Creates list of all loans present in database
            var loans = from loan in _context.Loans
                        select loan;

            // Adds donated book title to view
            await loans.ForEachAsync(loan =>
            {
                var book = books.FirstOrDefault(book => book.BookID == loan.BookID);
                if (book != null)
                {
                    loan.title = book.Title;
                }
            });

            // Searches loans by ISBN is search query is present
            if (!String.IsNullOrEmpty(searchString))
            {
                loans = loans.Where(s => s.ISBN.Contains(searchString));
            }

            // Orders loans by date loaned
            loans = loans.OrderByDescending(loans => loans.LoanDate);

            int pageSize = 11; // Amount of content to display on page
            return View(await PaginatedList<Loans>.CreateAsync(loans, pageNumber ?? 1, pageSize));
        }

        // GET: Loans/Create
        [Route("Loans/Create/{isbn?}")]
        public IActionResult Create(String? isbn)
        {
            // Display Loan create form
            return View();
        }

        // POST: Loans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Loans/Create/{isbn?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LoanerName,LoanerEmail,LoanerPhone,ISBN,LoanDate")] Loans loans)
        {
            var book = await _context.Book
                                    .FirstOrDefaultAsync(x => x.ISBN13 == loans.ISBN || x.ISBN10 == loans.ISBN);

            // Sends an error to the user if book does not exist
            if (book == null)
            {
                ModelState.AddModelError("ISBN", "Can not find book in database");
            }

            if (ModelState.IsValid)
            {
                loans.Id = Guid.NewGuid(); // Creates a new GUID
                loans.LoanDate = DateTime.Now; // Sets date to now
                loans.BookID = book.BookID;

                _context.Add(loans); // Adds new Loan to database
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Page));
            }
            return View(loans);
        }

        // GET: Loans/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            // If id is not present a not found page is returned
            if (id == null || _context.Loans == null)
            {
                return NotFound();
            }

            // Queries loan table by id and displays loan info
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
            // Confirms loan table exists 
            if (_context.Loans == null)
            {
                return Problem("Entity set 'BookExchangeContext.Loans'  is null.");
            }

            // Finds loan by ID and deletes it
            var loans = await _context.Loans.FindAsync(id);
            if (loans != null)
            {
                _context.Loans.Remove(loans);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Page));
        }

        // Confirms that loan exists in database.
        private bool LoansExists(Guid id)
        {
            return (_context.Loans?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }

    internal class Bob
    {
        public string Title { get; set; }
        public Guid ID { get; set; }
    }
}
