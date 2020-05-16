using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Helpers;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ExpenseTracker.Controllers
{
    public class UserController : Controller
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IConfiguration _configuration;
        public UserController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _accessor = httpContextAccessor;
            _configuration = configuration;
        }


        // GET: User
        public IActionResult Index()
        {
            return View();
        }


        //**************CREATES NEW USER************** 
        #region Create New User
        // GET: User/Create
        public IActionResult CreateUser()
        {
            return View("CreateUser", new User());
        }

        //collects new user information 
        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUser(User user)
        {
            //checks for errors in user data
            bool isError = false;

            UserDB userDB = new UserDB(_configuration);

            //At this point and For testing purposes I am only validating the username and password 
            //Ensures the username is not null
            if (user.userName == null)
            {
                ModelState.AddModelError("UserName", "Username Must Be Valid");
                isError = true;
            }
            //ADD need to have a check to see if the username is unique or not
            if (user.userName != null)
            {
                bool not_unique = userDB.CheckIfUsernameAvalible(user);
                if (not_unique)
                {
                    ModelState.AddModelError("UserName", "Choose A Unique Username");
                    isError = true;
                }
            }

            //Ensures the password is null
            if (user.password == null)
            {
                ModelState.AddModelError("password", "Password must be significant");
                isError = true;
            }

            //if there is an error create user view is returned
            if (isError)
            {
                return View("CreateUser", user);
            }
            else
            {
                //calls a stored procedure that stores the new user data in a table 
                userDB.StoreUserInDbTable(user);

                return View("CreateSuccess");
            }
        }

        
        #endregion

        //**************USER LOGIN METHODS************** 
        #region User Login Methods
        public User GetUserFromDataBase(User user)
        {
            User ReturnUser = new User();
            UserDB userDB = new UserDB(_configuration);
            ReturnUser = userDB.GetUserFromDataBase(user);
            
            _accessor.HttpContext.Session.SetObjectAsJson("LoggedInUser", ReturnUser);
            return ReturnUser;
        }
        #endregion

        //**************EDITS USER ************** 
        #region Edit User
        // GET: Dashboard/Edit
        public ActionResult EditUser(int IDNumber)
        {
            var temp = _accessor.HttpContext.Session.GetObjectFromJson<User>("LoggedInUser");
            return View(temp);
        }

        // POST: Dashboard/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(User user)
        {
            
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Dashboard", "User");
            }
            catch
            {
                return View();
            }
        }
        

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View();
            }
        }
        #endregion

        //need a method that allows user to edit user information this would override user in the database


        //need a method that allows the user to remove all expense records and user profile //need a method that displays company profile 

        //need a method that displays user information

        //need a method that calls edit user

        //************** USER DASHBOARD************** 
        #region User Dashboard
        public IActionResult Dashboard(User user)
        {
            ViewBag.User = user;
            return View(user);
        }
        #endregion

        [HttpGet]
        public IActionResult CreateEmployerUser()
        {
            return View(new User());
        }

        [HttpPost]
        public IActionResult CreateEmployerUser(User user)
        {

            return View(new User());
        }
    }
}