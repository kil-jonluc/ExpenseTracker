using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Models
{
    public class Expense
    {
        public int Id { get; set; }
        [DisplayName("Expense Description")]
        public string Description { get; set; }

        [DisplayName("Project")]
        public string Project { get; set; }

        [DisplayName("Expense Date")]
        public DateTime Date { get; set; }

        [DisplayName("Category")]
        public string Category { get; set; }
        
        [DisplayName("Merchant Name")]
        public string Merchant { get; set; }

        [DisplayName("Amount")]
        public decimal Amount { get; set; }

        [DisplayName("Report Number")]
        public string ReportNumber { get; set; }
        public int EmployerId { get; set; }
        public Statuses Status { get; set; }

        [DisplayName("Employee Name")]
        public string EmployeeName { get; set; } = " ";
        public int UserID { get; set; }
    }
}
