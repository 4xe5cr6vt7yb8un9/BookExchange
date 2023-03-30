using Microsoft.AspNetCore.Mvc;

namespace BookExchange.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly ILogger<BookController> _logger;

        public BookController(ILogger<BookController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetBookByISBN")]
        public Book Get()
        {
            return new Book();
        }
    }
}