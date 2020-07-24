using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Models
{
    public enum Statuses
    {
        [Display(Description ="With Employer")]
        WithEmployer = 0,
        
        [Display(Description ="With Employee")]
        WithEmployee = 1,
        
        [Display(Description ="Under Review")]
        UnderReview = 2,
        
        [Display(Description ="Approved")]
        Approved = 3,
        
        [Display(Description ="Denied")]
        Denied = 4
    }
}
