using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{

    //Controls the functionality of the user Dashboard
    public class DashboardController : Controller
    {
       private User _user;
       // public DashboardController(User user)
       // {
       //     _user = user; 
       // }
        //Calling the method Index from the controller class returns the view named dashboard
        public IActionResult Dashboard(User user)
        {
            _user = user;
            ViewBag.User = user;
            return View(_user);
        }

        //returns the Edit User view 
        public IActionResult EditUser()
        {

            TempData["User"] = _user;
            return RedirectToAction("EditUser", "User");
        }
        //need a method that displays company profile 

        //need a method that displays user information

        //need a method that calls edit user

    }
}