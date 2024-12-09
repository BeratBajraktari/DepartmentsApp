using DepartmentsApp.Models;
using DepartmentsApp.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace DepartmentsApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserRepository _userRepository;

        public AuthController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _userRepository.FindByUsername(username);
            if (user != null && VerifyPasswordHash(password, user.PasswordHash))
            {
                HttpContext.Session.SetString("Username", username);
                ViewBag.IsAuthenticated = true;  
                ViewBag.Username = username;     
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid login credentials.";
            return View();
        }


        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(string username, string password)
        {
            var user = _userRepository.FindByUsername(username);
            if (user == null)
            {
                var passwordHash = HashPassword(password);
                _userRepository.Register(new User { Username = username, PasswordHash = passwordHash });
                return RedirectToAction("Login");
            }

            ViewBag.Error = "Username already exists.";
            return View();
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Username");
            ViewBag.IsAuthenticated = false; 
            return RedirectToAction("Login");
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            return HashPassword(password) == storedHash;
        }
    }
}

