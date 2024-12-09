using System.Diagnostics;
using DepartmentsApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace DepartmentsApp.Controllers
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
            var isAuthenticated = HttpContext.Session.GetString("Username") != null;
            ViewBag.IsAuthenticated = isAuthenticated;

            if (isAuthenticated)
            {
                ViewBag.Username = HttpContext.Session.GetString("Username");
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
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
