using BookExchange.Controllers;
using Microsoft.AspNetCore.Mvc;

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
            Book ISBNBook = SQLGetActions.getBookByQuery("ISBN = "+ISBN);
            return ISBNBook;
        }

        [HttpGet("TITLE/{Title}")]
        public Book GetTitleBook(String Title)
        {
            Book TitleBook = SQLGetActions.getBookByQuery("Title = " + Title);
            return TitleBook;
        }
    }

    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        [HttpGet("UserID/{UserID}")]
        public ExchangeUser GetIDUser(String UserID)
        {
            ExchangeUser User = SQLGetActions.getUserByQuery("UserID = " + UserID);
            return User;
        }

        [HttpGet("Email/{Email}")]
        public ExchangeUser GetEmailUser(String Email)
        {
            ExchangeUser User = SQLGetActions.getUserByQuery("Email = " + Email);
            return User;
        }
    }
}