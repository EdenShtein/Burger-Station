using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Burger_Station.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            string user = HttpContext.Session.GetString("Type");
            if (user == null || user != "Admin")
            {
                return RedirectToAction("Login", "Users");
            }
            return View();
        }
        public IActionResult Signup()
        {
            string user = HttpContext.Session.GetString("Type");
            if (user == null || user != "Admin")
            {
                return RedirectToAction("Signup", "Users");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}