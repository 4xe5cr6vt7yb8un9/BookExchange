using Microsoft.AspNetCore.Mvc;
using BookExchange.Models;
using System.Diagnostics;

namespace BookExchange.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Displays Home page
        public IActionResult Index()
        {
            return View();
        }

        // Displays error page
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Display 404 error page
        [Route("/PageNotFound")]
        public IActionResult PageNotFound()
        {
            return View();
        }

    }
}