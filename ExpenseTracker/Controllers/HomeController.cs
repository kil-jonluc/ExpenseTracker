using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Http;
using ExpenseTracker.Helpers;
using System;
using Microsoft.Extensions.Configuration;

namespace ExpenseTracker.Controllers
{
    //Controls the functionality of the home page views 
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IConfiguration _configuration;

        public HomeController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _accessor = httpContextAccessor;
            _configuration = configuration;
        }

        
        //returns the index view 
        public IActionResult Index()
        {
            return View();
        }

        //returns the privacy view 
        public IActionResult Privacy()
        {
            return View();
        }

        //error message I guess
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //**************LOGIN AND PULLS USER CLASS FROM DATABASE************** 
        // GET: Home/LoinIn
        public IActionResult Login()
        {        
            //login view returns the view of login with a new user class as a parameter
            return View("Login", new User());
        }

        // POST: Home/Login
        [HttpPost]
        public IActionResult Login(User user)
        {
            //bool that makes sure all the errors are cleared out to log in
            bool isError = false;
            
            //this requires the user to enter a user name, it can't be null
            if (user.userName == null)
            {
                //this is what sends the error messsage 
                ModelState.AddModelError("userName", "User Name Must significant");
                isError = true;
            }
            //this requires the user to enter a password, it can't be null
            if (user.password == null)
            {
                //this is what sends the error messsage 
                ModelState.AddModelError("password", "Password must be significant");
                isError = true;
            }
            //returns back to the login page if there is an error
            if (isError)
            {
                return View("Login", user);
            }
            else
            {
                try
                {
                    //creates an instance of the user controller and then calls the method that retrieves the correct user from the data base
                    UserDB userDB = new UserDB(_configuration);
                    User RetrievedUser = userDB.GetUserFromDataBase(user);
                    if (RetrievedUser.IDNumber != 0)
                    {
                        _accessor.HttpContext.Session.SetObjectAsJson("LoggedInUser", RetrievedUser);
                        //if there is not error then the user is redirected to the dashboard
                        return RedirectToAction("Dashboard", "User", RetrievedUser);
                    }
                    else
                    {
                        ViewBag.Message = "user not found";
                        //otherwise this goes back to the login page
                        return Login();
                    }
                    
                }
                catch(Exception ex)
                {
                    //otherwise this goes back to the login page
                    return Login();
                }

            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            // clear anything in session on logout
            _accessor.HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
