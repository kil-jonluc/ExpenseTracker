using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Helpers
{
    public static class PageHelpers
    {
        public static IEnumerable<SelectListItem> ToSelectList(this IEnumerable<Employer> employers)
        {
            if(employers == null)
            {
                return null;
            }
            else
            {
                return employers.Select(employer => employer.ToSelectList());
            }
        }

        public static SelectListItem ToSelectList(this Employer employer)
        {
            if (employer == null)
            {
                return null;
            }
            else
            {
                return new SelectListItem(employer.CompanyName, employer.Id.ToString());
            }
        }
    }
}
