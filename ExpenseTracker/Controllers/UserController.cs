using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    public class UserController : Controller
    {
        SqlDataReader reader = null;
        SqlConnection connection;
        string connectionString = null;

        // GET: User
        public IActionResult Index()
        {
            return View();
        }

        // GET: User/Create
        public IActionResult CreateUser()
        {
            return View("CreateUser", new User());
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUser(User user)
        {
           // try
            {
                bool isError = false;
                if (user.userName == null)
                {
                    ModelState.AddModelError("UserName", "Username Must Be Valid");
                    isError = true;
                }
                //ADD need to have a check to see if the username is unique or not
                if (user.userName != null)
                {
                }
                else
                {
                    ModelState.AddModelError("username", "Username must be significant");
                    isError = true;
                }
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

                    //returns a view that tells user that new user was created
                    return View("CreateSuccess");
                    //return RedirectToAction("Index", "Home");
                }
            }
            //catch
           // {
            //    return View();
           // }
        }

        protected void StoreUserInDbTable(User user)
        {
            connectionString = @"Data Source = (localdb)\ProjectsV13; Initial Catalog = ExpenseTrackerDataBase; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";
            connection = new SqlConnection(connectionString);

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

                reader = cmd.ExecuteReader();
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

        protected int CheckIfUsernameAvalible(User user)
        {//not working yet
            connectionString = @"Data Source = (localdb)\ProjectsV13; Initial Catalog = ExpenseTrackerDataBase; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";
            connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("spCheckingUsername", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserName", user.userName);
                reader = cmd.ExecuteReader();
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
            //placeholder for now
            return 1;
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
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

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        //need a method that allows user to edit user information this would override user in the database


        //need a method that allows the user to remove all expense records and user profile
    }
}