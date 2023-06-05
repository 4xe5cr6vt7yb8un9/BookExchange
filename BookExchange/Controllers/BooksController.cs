using BookExchange.Actions;
using BookExchange.Data;
using BookExchange.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

            // Returns to the first page when searching books
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            // Queries database for books
            var books = from book in _context.Book
                        select book;

            // If search query is not empty, sorts books by Title and Author
            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.Title.Contains(searchString)
                                       || s.Author.Contains(searchString));
            }

            // Sorts book list by desired sort order
            books = sortOrder switch
            {
                "name_desc" => books.OrderByDescending(book => book.Title),
                "Date" => books.OrderBy(book => book.Published),
                "date_desc" => books.OrderByDescending(book => book.Published),
                _ => books.OrderBy(book => book.Title),
            };

            int pageSize = 6; // Amount of items to display on page

            // Creates a Paged List of book model and casts to view
            return View(await PaginatedList<Book>.CreateAsync(books.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Books/Details/5
        [Route("Books/Details/{id?}")]
        public async Task<IActionResult> Details(Guid? id, String? isbn)
        {
            // If ID and ISBN variables are null a not found page is returned
            if (id == null && isbn == null || _context.Book == null)
            {
                return NotFound();
            }

            // If ID variable is present 
            // Finds book details using id
            if (id != null)
            {
                // Queries database by book id
                var book = await _context.Book
                    .FirstOrDefaultAsync(m => m.BookID == id);
                if (book == null)
                {
                    return NotFound();
                }
                return View(book);
            }

            // If ISBN variable is present 
            // Finds book details using isbn
            if (isbn != null)
            {
                // Queries database by book ISBN
                var book = await _context.Book
                    .FirstOrDefaultAsync(m => m.ISBN == isbn);
                if (book == null)
                {
                    return NotFound();
                }
                return View(book);
            }

            // If all fails returns not found page
            return NotFound();
        }

        // GET: Books/Create
        [Route("Books/Create/{book?}")]
        public IActionResult Create(Book? book)
        {
            // Displays book creation form
            return View(book);
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkBookID=317598.
        [HttpPost("Books/Create/{book?}"), ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost([Bind("BookID,Title,Author,Description,Published,ISBN,Available")] Book book)
        {
            // Confirms if required variables are present 
            if (ModelState.IsValid)
            {
                book.BookID = Guid.NewGuid(); // Creates a new GUID
                _context.Add(book); // Adds Book to database
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Page)); // Redirects to book list page
            }
            // Returns errors to user if present
            return View(book);
        }

        // GET: Books/CreateISBN
        [Route("Books/CreateISBN")]
        public IActionResult CreateISBN()
        {
            // Shows Create ISBN form
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkBookID=317598.
        [HttpPost("Books/CreateISBN")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateISBN(String ISBN)
        {
            // Checks if book exists in Google Database
            Boolean exists = Actions.ISBNScraper.bookExists(ISBN);

            // If the book does not exist in database an error is return to user
            if (!exists)
            {
                ModelState.AddModelError(nameof(ISBN), "Unable to find book information");
            }

            // Queries database if book was already added
            var dataBook = await _context.Book
                    .FirstOrDefaultAsync(m => m.ISBN == ISBN);

            // If the book already exists in the database an error is returned
            if (dataBook != null)
            {
                ModelState.AddModelError(nameof(ISBN), "Book already exists in database");
            }

            if (ModelState.IsValid)
            {
                // Grabs data from Google Book database and redirects to create form
                Book? book = ISBNScraper.ISBNGrabAsync(ISBN).Result;

                return RedirectToAction(nameof(Create), book);
            }
            // Returns to Create ISBN form is there was an error
            return View();
        }

        // GET: Books/Edit/5
        [Route("Books/Edit/{id}")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            // If the ID does not exist a not found page is returned
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            // Searches database for book info and displays it to user
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
        [HttpPost("Books/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("BookID,Title,Author,Description,Published,ISBN,Available")] Book book)
        {
            // If given ID is different from the book id an error is displayed
            if (id != book.BookID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try // Attempts to update database with changes
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
                // Returns to book page after edit
                return RedirectToAction(nameof(Details), id);
            }
            return View(book);
        }

        // GET: Books/Delete/5
        [Route("Books/Delete/{id}")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            // Return not found error is book id is not given
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            // Collects and displays given book info
            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.BookID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost("Books/Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            // Confirms the book table exists in database
            if (_context.Book == null)
            {
                return Problem("Entity set 'BookExchangeContext.Book'  is null.");
            }

            // Finds book in database and removes it
            var book = await _context.Book.FindAsync(id);
            if (book != null)
            {
                _context.Book.Remove(book);
            }

            // Confirms changes and redirects to book index
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Page));
        }

        // Confirms a book exists
        private bool BookExists(Guid id)
        {
            return (_context.Book?.Any(e => e.BookID == id)).GetValueOrDefault();
        }
    }
}
