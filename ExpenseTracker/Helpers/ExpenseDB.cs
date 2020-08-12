using ExpenseTracker.Models;
using Microsoft.AspNetCore.Razor.Language.Extensions;
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

        public void InsertExpense(Expense expense, int UserID)
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
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@Status", expense.Status);

                    //if rows change is value then you know that it this worked correctly
                    int rowsChanged = cmd.ExecuteNonQuery();
                }
            }catch(Exception ex)
            {
                var temp = ex.Message;
            }
        }

        public void UpdateExpense(Expense expense)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("Expense_Update", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", expense.Id);
                    cmd.Parameters.AddWithValue("@Description", expense.Description);
                    cmd.Parameters.AddWithValue("@Project", expense.Project);
                    cmd.Parameters.AddWithValue("@Date", expense.Date);
                    cmd.Parameters.AddWithValue("@Category", expense.Category);
                    cmd.Parameters.AddWithValue("@Merchant", expense.Merchant);
                    cmd.Parameters.AddWithValue("@Amount", expense.Amount);
                    cmd.Parameters.AddWithValue("@ReportNumber", expense.ReportNumber);
                    cmd.Parameters.AddWithValue("@Status", expense.Status);


                    //if rows change has a value then you know that it this worked correctly
                    int rowsChanged = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                var temp = ex.Message;
            }
        }
        public void UpdateExpenseStatus(Expense expense)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("Expense_StatusUpdate", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", expense.Id);
                    cmd.Parameters.AddWithValue("@Status", expense.Status);

                    //if rows change has a value then you know that it this worked correctly
                    int rowsChanged = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                var temp = ex.Message;
            }
        }


        public Expense GetExpenseById(int expenseId)
        {
            Expense expense = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("Expense_GetById", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", expenseId);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        expense = new Expense()
                        {
                            Id = reader.GetInt32(0),
                            Description = reader.GetString(1),
                            Project = reader.GetString(2),
                            Date = reader.GetDateTime(3),
                            Category = reader.GetString(4),
                            Merchant = reader.GetString(5),
                            Amount = reader.GetDecimal(6),
                            ReportNumber = reader.GetString(7),
                            EmployerId = reader.GetInt32(8),
                            Status = (Statuses)reader.GetInt32(10)
                        };
                    }
                }
            }

            return expense;
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
                            Id = reader.GetInt32(0),
                            Description = reader.GetString(1),
                            Project = reader.GetString(2),
                            Date = reader.GetDateTime(3),
                            Category = reader.GetString(4),
                            Merchant = reader.GetString(5),
                            Amount = reader.GetDecimal(6),
                            ReportNumber = reader.GetString(7),
                            EmployerId = reader.GetInt32(8),
                            UserID = reader.GetInt32(9),
                            Status = (Statuses)reader.GetInt32(10)
                        });

                    }
                }
            }
            return expenses;
        }

        public IEnumerable<Expense> GetExpenseByUserID(int UserID)
        {
            List<Expense> expenses = new List<Expense>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("Expense_GetAllByUserId", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", UserID);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        expenses.Add(new Expense()
                        {
                            Id = reader.GetInt32(0),
                            Description = reader.GetString(1),
                            Project = reader.GetString(2),
                            Date = reader.GetDateTime(3),
                            Category = reader.GetString(4),
                            Merchant = reader.GetString(5),
                            Amount = reader.GetDecimal(6),
                            ReportNumber = reader.GetString(7),
                            EmployerId = reader.GetInt32(8),
                            Status = (Statuses)reader.GetInt32(10)
                        });

                    }
                }
            }
            return expenses;
        }

        public void DeleteExpense(int expenseId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("Expense_Delete", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", expenseId);

                int rowsChanged = cmd.ExecuteNonQuery();
            }
        }
    }
}
