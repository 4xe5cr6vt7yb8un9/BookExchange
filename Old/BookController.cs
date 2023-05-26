using BookExchange.Actions;
using BookExchange.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BookExchange.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
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
                newBook = ISBNScraper.ISBNGrab(ISBN);
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

        [HttpPost]
        public ExchangeUser AddNewUser(ExchangeUser userInfo)
        {
            ExchangeUser newUser = new()
            {
                Name = userInfo.Name,
                Email = userInfo.Email
            };
            SQLSetActions.addUser(newUser);
            return newUser;
        }
    }
}