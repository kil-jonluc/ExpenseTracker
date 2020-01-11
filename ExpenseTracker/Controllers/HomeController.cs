using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ExpenseTracker.Models;

namespace ExpenseTracker.Controllers
{
    //Controls the functionality of the home page views 
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // GET: Home/LoinIn
        public IActionResult Login()
        {
            
            return View("Login", new User());
        }

        // POST: Home/Login
        [HttpPost]
        public IActionResult Login(User user)
        {
            bool isError = false;
            if (user.userName == null)
            {
                ModelState.AddModelError("userName", "User Name Must significant");
                isError = true;
            }
            if (user.password == null)
            {
                ModelState.AddModelError("password", "Password must be significant");
                isError = true;
            }
            if (isError)
            {
                return View("Login", user);
            }
            else
            {
                try
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                catch
                {
                    return View();
                }
            }
        }
    }
}
