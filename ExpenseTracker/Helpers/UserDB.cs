using ExpenseTracker.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Helpers
{
    public class UserDB
    {

        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public UserDB(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("ExpenseTracker");
        }

        public User GetUserFromDataBase(User user)
        {
            User ReturnUser = new User();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("spRetrieve_User", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserName", user.userName);
                cmd.Parameters.AddWithValue("@Password", user.password);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ReturnUser.IDNumber = reader.GetInt32(0);
                    ReturnUser.FirstName = reader.GetString(1);
                    ReturnUser.LastName = reader.GetString(2);
                    ReturnUser.email = reader.GetString(3);
                    ReturnUser.userName = reader.GetString(4);
                    ReturnUser.password = reader.GetString(5);
                    ReturnUser.phoneNumber = reader.GetString(6);
                    ReturnUser.SSN = reader.GetString(7);
                    // for backward compatability, we can't assume all users have a role Id
                    // If they don't have an Id, make them an employee
                    if(reader.GetValue(8) != DBNull.Value)
                    {
                        ReturnUser.RoleId = reader.GetInt32(8);
                    }
                    else
                    {
                        ReturnUser.RoleId = 1;
                    }
                    ReturnUser.EmployerId = reader.GetInt32(9);
                }
            }
            return ReturnUser;
        }

        public User GetUserById(int userId)
        {
            User ReturnUser = new User();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("User_GetById", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", userId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ReturnUser.IDNumber = reader.GetInt32(0);
                    ReturnUser.FirstName = reader.GetString(1);
                    ReturnUser.LastName = reader.GetString(2);
                    ReturnUser.email = reader.GetString(3);
                    ReturnUser.userName = reader.GetString(4);
                    ReturnUser.password = reader.GetString(5);
                    ReturnUser.phoneNumber = reader.GetString(6);
                    ReturnUser.SSN = reader.GetString(7);
                    // for backward compatability, we can't assume all users have a role Id
                    // If they don't have an Id, make them an employee
                    if (reader.GetValue(8) != DBNull.Value)
                    {
                        ReturnUser.RoleId = reader.GetInt32(8);
                    }
                    else
                    {
                        ReturnUser.RoleId = 1;
                    }
                    ReturnUser.EmployerId = reader.GetInt32(9);
                }
            }
            return ReturnUser;
        }

        //**************STORES NEW USER IN DATA BASE************** 
        public void StoreUserInDbTable(User user)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
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
                    cmd.Parameters.AddWithValue("@RoleId", user.RoleId);
                    cmd.Parameters.AddWithValue("@EmployerId", user.EmployerId);

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
        public Boolean CheckIfUsernameAvalible(User user)
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
                using (SqlConnection connection = new SqlConnection(_connectionString))
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
    }
}
