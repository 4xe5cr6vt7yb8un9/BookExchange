using BookExchange.Actions;
using BookExchange.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BookExchange.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {

        /*
        private readonly ILogger<BookController> _logger;

        public BookController(ILogger<BookController> logger)
        {
            _logger = logger;
        }*/

        [HttpGet("ISBN/{ISBN}")]
        public Book GetISBNBook(String ISBN)
        {
            Book ISBNBook = SQLGetActions.getBookByQuery("ISBN = " + ISBN);
            return ISBNBook;
        }

        [HttpGet("TITLE/{Title}")]
        public Book GetTitleBook(String Title)
        {
            Book TitleBook = SQLGetActions.getBookByQuery("Title = '" + Title + "'");
            return TitleBook;
        }

        [HttpPost]
        public Book AddBook(Book Info)
        {
            Book newBook = new()
            {
                ISBN = Info.ISBN.Replace("-", String.Empty),
                Title = Info.Title,
                Author = Info.Author,
                Description = Info.Description,
                Published = Info.Published,
                Available = Info.Available
            };
            SQLSetActions.addBook(newBook);
            return newBook;
        }

        [HttpGet("New/{ISBN}")]
        public Book AddBookByISBN(String ISBN)
        {
            Book newBook;
            if (!SQLGetActions.verifyISBN(ISBN))
            {
                newBook = ISBNScraper.Scrap(ISBN);
                SQLSetActions.addBook(newBook);
            }

            return GetISBNBook(ISBN);
        }
    }

    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        [HttpGet("UserID/{UserID}")]
        public ExchangeUser GetIDUser(String UserID)
        {
            ExchangeUser User = SQLGetActions.getUserByQuery("UserID = '" + UserID + "'");
            return User;
        }

        [HttpGet("Email/{Email}")]
        public ExchangeUser GetEmailUser(String Email)
        {
            ExchangeUser User = SQLGetActions.getUserByQuery("Email = '" + Email + "'");
            return User;
        }
    }
}