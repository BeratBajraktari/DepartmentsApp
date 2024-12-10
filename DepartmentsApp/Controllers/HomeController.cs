using DepartmentsApp.Models;
using DepartmentsApp.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DepartmentsApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly INewsRepository _newsRepo;

        public HomeController(INewsRepository newsRepo)
        {
            _newsRepo = newsRepo;
        }

        public IActionResult Index(int page = 1)
        {
            var isAuthenticated = HttpContext.Session.GetString("Username") != null;
            ViewBag.IsAuthenticated = isAuthenticated;

            if (isAuthenticated)
            {
                ViewBag.Username = HttpContext.Session.GetString("Username");
            }

            int pageSize = 6;

            var news = _newsRepo.GetNewsByPage(page, pageSize);

            var totalNews = _newsRepo.GetAll().Count();
            var totalPages = (int)Math.Ceiling(totalNews / (double)pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(news);
        }


        public IActionResult Details(int id)
        {
            var currentNews = _newsRepo.GetById(id);

            if (currentNews == null)
            {
                return NotFound();
            }

            var lastNews = _newsRepo.GetLatestNewsExcludingCurrent(id);

            ViewBag.LastNews = lastNews;

            var isAuthenticated = HttpContext.Session.GetString("Username") != null;
            ViewBag.IsAuthenticated = isAuthenticated;

            if (isAuthenticated)
            {
                ViewBag.Username = HttpContext.Session.GetString("Username");
            }

            return View(currentNews);
        }
    }

}
