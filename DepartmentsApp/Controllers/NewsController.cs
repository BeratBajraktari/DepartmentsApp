using DepartmentsApp.Models;
using DepartmentsApp.Repository;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace DepartmentsApp.Controllers
{
    
    public class NewsController : Controller
    {
        private readonly INewsRepository _newsRepository;

        public NewsController(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public IActionResult Index()
        {
            var isAuthenticated = HttpContext.Session.GetString("Username") != null;
            ViewBag.IsAuthenticated = isAuthenticated;

            if (isAuthenticated)
            {
                ViewBag.Username = HttpContext.Session.GetString("Username");
            }

            var news = _newsRepository.GetAll();
            return View(news);
        }


        public IActionResult GetNews([DataSourceRequest] DataSourceRequest request)
        {
            var news = _newsRepository.GetAll();  

            return Json(news.ToDataSourceResult(request));
        }

        [HttpPost]
        public IActionResult CreateUpdate([DataSourceRequest] DataSourceRequest request, News news)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("ModelState Invalid.");
            }

            if (news.NewsId > 0)
                _newsRepository.Update(news);
            else
                _newsRepository.Add(news);
            
            return Json(ModelState.ToDataSourceResult());
        }

        public IActionResult Delete(News news)
        {
            _newsRepository.Remove(news.NewsId);
            return Json(Ok());
        }

        [HttpPost]
        public IActionResult SaveFile(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return Json("/uploads/" + file.FileName);
            }

            return BadRequest("Invalid file.");
        }
    }
}
