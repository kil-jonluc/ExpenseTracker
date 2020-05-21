using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Models
{
    public class UserViewModel
    {
        public User User { get; set; }
        public IEnumerable<Employer> Employers { get; set; }
    }
}
