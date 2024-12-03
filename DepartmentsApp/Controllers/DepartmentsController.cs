using DepartmentsApp.Models;
using DepartmentsApp.Repository;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace DepartmentsApp.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentRepository _depRepo;

        public DepartmentsController(IDepartmentRepository depRepo)
        {
            _depRepo = depRepo;
        }

        public IActionResult Index()
        {
            var isAuthenticated = HttpContext.Session.GetString("Username") != null;
            ViewBag.IsAuthenticated = isAuthenticated;

            if (isAuthenticated)
            {
                ViewBag.Username = HttpContext.Session.GetString("Username");
            }

            var departments = _depRepo.GetAll();
            return View(departments);
        }

        public IActionResult GetDepartments([DataSourceRequest] DataSourceRequest request)
        {
            var departments = _depRepo.GetAll();
            return Json(departments.ToDataSourceResult(request));
        }

        [HttpPost("create")]
        public JsonResult Create([DataSourceRequest] DataSourceRequest request, Department department)
        {
            if (ModelState.IsValid)
            {
                _depRepo.Add(department);
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            return Json(new[] { department }.ToTreeDataSourceResult(request, ModelState));
        }

        public JsonResult Update([DataSourceRequest] DataSourceRequest request, Department department)
        {
            if (ModelState.IsValid)
            {
                _depRepo.Update(department);
            }

            return Json(new[] { department }.ToTreeDataSourceResult(request, ModelState));
        }

        [HttpPost("destroy")]
        public JsonResult Destroy([DataSourceRequest] DataSourceRequest request, Department department)
        {
            if (department.DepartmentId != 0)
            {
                _depRepo.Remove(department.DepartmentId);
            }
            else
            {
                return Json(new { error = "Invalid request" });
            }

            return Json(new[] { department }.ToTreeDataSourceResult(request, ModelState));
        }
    }
}
