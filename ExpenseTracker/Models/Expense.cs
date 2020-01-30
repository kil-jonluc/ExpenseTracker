using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Models
{
    public class Expense
    {
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
        public string Amount { get; set; }

        [DisplayName("Report Number")]
        public string ReportNumber { get; set; }






    }
}
