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

        // Index action to render the Departments view
        public IActionResult Index()
        {
            // Checking if the session contains a Username
            var isAuthenticated = HttpContext.Session.GetString("Username") != null;
            ViewBag.IsAuthenticated = isAuthenticated;

            // If authenticated, show the Username in the view
            if (isAuthenticated)
            {
                ViewBag.Username = HttpContext.Session.GetString("Username");
            }

            // Retrieve the departments from the repository
            var departments = _depRepo.GetAll();
            return View(departments);
        }

        // Get the list of departments for the Kendo TreeList
        public IActionResult GetDepartments([DataSourceRequest] DataSourceRequest request)
        {
            var departments = _depRepo.GetAll();
            return Json(departments.ToDataSourceResult(request));
        }

        // Handle creation of new department via AJAX (POST request)
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

            // Returning the created department to Kendo TreeList
            return Json(new[] { department }.ToTreeDataSourceResult(request, ModelState));
        }

        // Update the department details via AJAX (POST request)
        public JsonResult Update([DataSourceRequest] DataSourceRequest request, Department department)
        {
            if (ModelState.IsValid)
            {
                _depRepo.Update(department);
            }

            // Returning the updated department to Kendo TreeList
            return Json(new[] { department }.ToTreeDataSourceResult(request, ModelState));
        }

        // Delete a department via AJAX (POST request)
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

            // Returning the result after deletion to Kendo TreeList
            return Json(new[] { department }.ToTreeDataSourceResult(request, ModelState));
        }
    }
}
