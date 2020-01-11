using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    //Controls the functionality of the user Dashboard
    public class DashboardController : Controller
    {
        //Calling the method Index from the controller class returns the view named dashboard
        public IActionResult Index()
        {
            return View("Dashboard");
        }
    }
}