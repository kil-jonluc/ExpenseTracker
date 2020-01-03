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
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
      
        //public IActionResult Index()
        //{
        //    return View();
        //}
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

        public IActionResult CreateUser()
        {
            
            return View("CreateUser", new User());
        }
        [HttpPost]
        public IActionResult CreateUser(User user)
        {
            bool isError = false;
            if (user.FirstName != "Jon")
            {
                ModelState.AddModelError("FirstName", "First Name Must Be Jon");
                isError = true;
            }
            if (user.SSN == null)
            {
                ModelState.AddModelError("SSN", "SSN must be significant");
                isError = true;
            }
            if (isError)
            {
                return View("CreateUser", user);
            }
            else
            {
                return View("Index");
            }
        }
    }
}
