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

namespace ExpenseTracker.Controllers
{
    public class UserController : Controller
    {
        private readonly IHttpContextAccessor _accessor;
        public UserController(IHttpContextAccessor httpContextAccessor)
        {
            _accessor = httpContextAccessor;
        }

        //connection string to the database
        //const string connectionString = @"Data Source=MICHAELCOMPUTER\SQLSERVER2016;Initial Catalog=ExpenseTrackerDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False";
        const string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ExpenseTrackerDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

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
                bool not_unique = CheckIfUsernameAvalible(user);
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
                StoreUserInDbTable(user);

                return View("CreateSuccess");
            }
        }

        //**************STORES NEW USER IN DATA BASE************** 
        protected void StoreUserInDbTable(User user)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataReader reader = null;

                try
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("spNewUser_Insert", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", user.LastName);
                    cmd.Parameters.AddWithValue("@Email", user.email);
                    cmd.Parameters.AddWithValue("@UserName", user.userName);
                    cmd.Parameters.AddWithValue("@Password", user.password);
                    cmd.Parameters.AddWithValue("@PhoneNumber", user.phoneNumber);
                    cmd.Parameters.AddWithValue("@SSN", user.SSN);

                    //if rows change is value then you know that it this worked correctly
                    int rowsChanged = cmd.ExecuteNonQuery();
                }
                finally
                {

                    //Close the connections 
                    if (connection != null)
                    {
                        connection.Close();
                    }
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
        }

        //**************KEEPS USER DATA FROM BEING DUPLICATED IN DATA BASE************** 
        protected Boolean CheckIfUsernameAvalible(User user)
        {
            try
            {
                //checks to see if the database exists and is connecting, does not really have any function other than debugging 
                //  string query = "select * from sysobjects where type='P' and name='spCheckingUsername'";
                //  bool spExists = false;
                //  using (SqlConnection connection = new SqlConnection(connectionString))
                //  {
                //      connection.Open();
                //      using (SqlCommand command = new SqlCommand(query, connection))
                //      {
                //          using (SqlDataReader reader = command.ExecuteReader())
                //          {
                //              while (reader.Read())
                //              {
                //                  spExists = true;
                //                  break;
                //              }
                //          }
                //      }
                //  }
                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    connection.Open();
                    SqlCommand cmd = new SqlCommand("spCheckingUsername", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserName", user.userName);

                    SqlDataReader reader = cmd.ExecuteReader();
                    int userNamesCount = 0;
                    if (reader.HasRows)
                    {
                        try
                        {
                            //there will be an error if there is no user name because it will try to define nothing to the intiger
                            while (reader.Read())
                            {
                                userNamesCount = reader.GetInt32(0);
                            }
                            return (userNamesCount == 0 ? false : true);
                        }
                        catch
                        {
                            return false;
                        }
                    }
                    else
                    {
                        //this would mean thought that the table does not exist and that one might need to be set up?
                        return false;
                    }

                    //below is some code to help me pull user data from the table using methods similar to the one above
                    //int userNamesCount = 0;
                    //User newUser = null;
                    //if (reader.HasRows)
                    //{
                    //    newUser = new User();
                    //    newUser.FirstName = reader.GetString(0);
                    //    newUser.LastName = reader.GetString(1);
                    //    userNamesCount = reader.GetInt32(0);
                    //    //string userName = reader.GetString(1);
                    //}
                }
            }
            finally
            {
            }
        }
        #endregion

        //**************USER LOGIN METHODS************** 
        #region User Login Methods
        public User GetUserFromDataBase(User user)
        {
            User ReturnUser = new User();
            UserDB userDB = new UserDB();
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
    }
}