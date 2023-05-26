using BookExchange.Actions;
using BookExchange.Data;
using BookExchange.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace BookExchange.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookExchangeContext _context;

        public BooksController(BookExchangeContext context)
        {
            _context = context;
        }

        // GET: Books
        [Route("Books/{pageNumber?}/{searchString?}")]
        public async Task<IActionResult> Page(string searchString, string sortOrder, string currentFilter, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var books = from book in _context.Book
                        select book;

            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.Title.Contains(searchString)
                                       || s.Author.Contains(searchString));
            }

            books = sortOrder switch
            {
                "name_desc" => books.OrderByDescending(book => book.Title),
                "Date" => books.OrderBy(book => book.Published),
                "date_desc" => books.OrderByDescending(book => book.Published),
                _ => books.OrderBy(book => book.Title),
            };

            int pageSize = 6;
            return View(await PaginatedList<Book>.CreateAsync(books.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Books/Details/5
        [Route("Books/Details/{id?}")]
        public async Task<IActionResult> Details(Guid? id, String? isbn)
        {
            if (id == null && isbn == null || _context.Book == null)
            {
                return NotFound();
            }
            if (id != null)
            {
                var book = await _context.Book
                    .FirstOrDefaultAsync(m => m.BookID == id);
                if (book == null)
                {
                    return NotFound();
                }
                return View(book);
            }
            if (isbn != null)
            {
                var book = await _context.Book
                    .FirstOrDefaultAsync(m => m.ISBN == isbn);
                if (book == null)
                {
                    return NotFound();
                }
                return View(book);
            }
            return NotFound();
        }

        // GET: Books/Create
        [Route("Books/Create/{book?}")]
        public IActionResult Create(Book? book)
        {
            return View(book);
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkBookID=317598.
        [HttpPost("Books/Create/{book?}"), ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost([Bind("BookID,Title,Author,Description,Published,ISBN,Available")] Book book)
        {
            if (ModelState.IsValid)
            {
                book.BookID = Guid.NewGuid();
                book.Available = 0;
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Page));
            }
            return View(book);
        }

        // GET: Books/Create
        [Route("Books/CreateISBN")]
        public IActionResult CreateISBN()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkBookID=317598.
        [HttpPost("Books/CreateISBN")]
        [ValidateAntiForgeryToken]
        public IActionResult CreateISBN(String ISBN)
        {
            Book? book = ISBNScraper.ISBNGrabAsync(ISBN).Result;

            return RedirectToAction(nameof(Create), book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkBookID=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("BookID,Title,Author,Description,Published,ISBN,Available")] Book book)
        {
            if (id != book.BookID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookID))
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
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.BookID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Book == null)
            {
                return Problem("Entity set 'BookExchangeContext.Book'  is null.");
            }
            var book = await _context.Book.FindAsync(id);
            if (book != null)
            {
                _context.Book.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Page));
        }

        private bool BookExists(Guid id)
        {
            return (_context.Book?.Any(e => e.BookID == id)).GetValueOrDefault();
        }
    }
}
