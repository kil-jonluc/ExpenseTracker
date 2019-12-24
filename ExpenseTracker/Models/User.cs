using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Models
{
    public class User
    {
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Email")]
        public string email { get; set; }
        
        [DisplayName("User Name")]
        public string userName { get; set; }

        [DisplayName("Password")]
        public string password { get; set; }
        
        [DisplayName("Phone Number")]
        public string phoneNumber { get; set; }
        
        [DisplayName("Social Security Number")]
        public string SSN { get; set; }
    }
}
