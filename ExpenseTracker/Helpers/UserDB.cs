﻿using ExpenseTracker.Models;
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
        //connection string to the database
        //const string connectionString = @"Data Source=MICHAELCOMPUTER\SQLSERVER2016;Initial Catalog=ExpenseTrackerDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False";
        const string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ExpenseTrackerDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


        public User GetUserFromDataBase(User user)
        {
            User ReturnUser = new User();
            using (SqlConnection connection = new SqlConnection(connectionString))
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
                        ReturnUser.Role = reader.GetInt32(8);
                    }
                    else
                    {
                        ReturnUser.Role = 1;
                    }
                }
            }
            return ReturnUser;
        }
    }
}
