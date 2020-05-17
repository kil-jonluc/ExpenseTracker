﻿using ExpenseTracker.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Helpers
{
    public class ExpenseDB
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public ExpenseDB(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("ExpenseTracker");
        }

        public void InsertExpense(Expense expense)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("Expense_Insert", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Description", expense.Description);
                    cmd.Parameters.AddWithValue("@Project", expense.Project);
                    cmd.Parameters.AddWithValue("@Date", expense.Date);
                    cmd.Parameters.AddWithValue("@Category", expense.Category);
                    cmd.Parameters.AddWithValue("@Merchant", expense.Merchant);
                    cmd.Parameters.AddWithValue("@Amount", expense.Amount);
                    cmd.Parameters.AddWithValue("@ReportNumber", expense.ReportNumber);
                    cmd.Parameters.AddWithValue("@EmployerId", expense.EmployerId);

                    //if rows change is value then you know that it this worked correctly
                    int rowsChanged = cmd.ExecuteNonQuery();
                }
            }catch(Exception ex)
            {
                var temp = ex.Message;
            }
        }

        public IEnumerable<Expense> GetExpensesByEmployer(int employerId)
        {
            List<Expense> expenses = new List<Expense>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("Expense_GetAllByEmployerId", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployerId", employerId);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        expenses.Add(new Expense()
                        {
                            Description = reader.GetString(1),
                            Project = reader.GetString(2),
                            Date = reader.GetDateTime(3),
                            Category = reader.GetString(4),
                            Merchant = reader.GetString(5),
                            Amount = reader.GetDecimal(6),
                            ReportNumber = reader.GetString(7),
                            EmployerId = reader.GetInt32(8)
                        });

                    }
                }
            }
            return expenses;
        }
    }
}
