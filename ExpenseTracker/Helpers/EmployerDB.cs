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
    public class EmployerDB
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public EmployerDB(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("ExpenseTracker");
        }

        public Employer GetEmployerById(int employerId)
        {
            Employer employer = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("Employer_GetById", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", employerId);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        employer = new Employer()
                        {
                            Id = reader.GetInt32(0),
                            CompanyName = reader.GetString(1)
                        };
                    }
                }
            }

            return employer;
        }

        public IEnumerable<Employer> GetAll()
        {
            List<Employer> employers = new List<Employer>(); ;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("Employer_GetAll", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        employers.Add(new Employer()
                        {
                            Id = reader.GetInt32(0),
                            CompanyName = reader.GetString(1)
                        });
                    }
                }
            }

            return employers;
        }
    }
}
