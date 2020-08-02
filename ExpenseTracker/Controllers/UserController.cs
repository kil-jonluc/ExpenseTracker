using ExpenseTracker.Helpers;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace ExpenseTracker.Controllers
{
    public class UserController : Controller
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IConfiguration _configuration;
        private readonly UserDB _userDB;
        public UserController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _accessor = httpContextAccessor;
            _configuration = configuration;
            _userDB = new UserDB(configuration);
        }

        //**************CREATES NEW USER************** 
        #region Create New User
        // GET: User/Create
        public IActionResult CreateUser()
        {
            UserViewModel userVM = new UserViewModel()
            {
                User = new User(),
                Employers = new EmployerDB(_configuration).GetAll()
            };
            return View(userVM);
        }

        //collects new user information 
        // POST: User/Create
        [HttpPost]
        public IActionResult CreateUser(User user)
        {
            //checks for errors in user data
            //bool isError = false;

            UserDB userDB = new UserDB(_configuration);
            user.RoleId = 0;

            //At this point and For testing purposes I am only validating the username and password 
            //Ensures the username is not null
            //if (user.userName == null)
            //{
            //    ModelState.AddModelError("UserName", "Username Must Be Valid");
            //    isError = true;
            //}
            //ADD need to have a check to see if the username is unique or not
            //if (user.userName != null)
            //{
            //    bool not_unique = userDB.CheckIfUsernameAvalible(user);
            //    if (not_unique)
            //    {
            //        ModelState.AddModelError("UserName", "Choose A Unique Username");
            //        isError = true;
            //    }
            //}
            //Ensures the password is null
            //if (user.password == null)
            //{
            //    ModelState.AddModelError("password", "Password must be significant");
            //    isError = true;
            //}
            //if there is an error create user view is returned
            //if (isError)
            //{
            //    return View("CreateUser", user);
            //}
            //else
            //{




            //calls a stored procedure that stores the new user data in a table 
            userDB.StoreUserInDbTable(user);
            return View("CreateSuccess");



            //}
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
        public ActionResult EditUser()         //public ActionResult EditUser(int IDNumber) not sure if the id parameter is needed 
        {
            var temp = _accessor.HttpContext.Session.GetObjectFromJson<User>("LoggedInUser");
            return View(temp);
        }

        // POST: Dashboard/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(User user)
        {


            if (ModelState.IsValid)
            {
                try
                {
                    _accessor.HttpContext.Session.SetObjectAsJson("LoggedInUser", user);
                    _userDB.UpdateUser(user);
                    return RedirectToAction("Dashboard", "User");
                }
                catch (Exception ex)
                {
                }
            }

            return View(user);

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
        public IActionResult Dashboard()
        {
            EmployerDB employerDB = new EmployerDB(_configuration);
            User user = _accessor.HttpContext.Session.GetObjectFromJson<User>("LoggedInUser");
            DashboardViewModel dashboardVM = new DashboardViewModel()
            {
                User = user,
                Employer = employerDB.GetEmployerById(user.EmployerId)
            };
            return View(dashboardVM);
        }
        #endregion

        [HttpGet]
        public IActionResult CreateEmployerUser()
        {
            UserViewModel userVM = new UserViewModel()
            {
                User = new User(),
                Employers = new EmployerDB(_configuration).GetAll()
            };
            return View(userVM);
        }

        [HttpPost]
        public IActionResult CreateEmployerUser(User user)
        {
            UserDB userDB = new UserDB(_configuration);
            // Set user a employer type (0), would prefer an enum but this works for now
            user.RoleId = 0;

            userDB.StoreUserInDbTable(user);
            return View("CreateSuccess");
        }
    }
}