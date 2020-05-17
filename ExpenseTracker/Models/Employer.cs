using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Models
{
    public class Employer
    {
        public int Id { get; set; }
        
        [DisplayName("Company Name")]
        public string CompanyName { get; set; }
    }
}
