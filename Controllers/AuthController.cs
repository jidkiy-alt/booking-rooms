using ASPNET_PROJECT.Data.Service;
using ASPNET_PROJECT.Data.Service.Interfaces;
using ASPNET_PROJECT.Models;
using ASPNET_PROJECT.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET_PROJECT.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _authService.RegisterAsync(model.Email, model.Password, model.FirstName, model.LastName);
            
            if (user == null)
            {
                ModelState.AddModelError("", "Пользователь с таким email уже существует");
                return View(model);
            }

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserName", $"{user.FirstName} {user.LastName}");
            
            return RedirectToAction("Index", "Room");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _authService.LoginAsync(email, password);
            
            if (user == null)
            {
                ViewBag.Error = "Неверный email или пароль";
                return View();
            }

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserName", $"{user.FirstName} {user.LastName}");
            
            return RedirectToAction("Index", "Room");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Room");
        }
    }
}