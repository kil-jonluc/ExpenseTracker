using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    public class SharedController : Controller
    {
        //list of user expenses
        protected List<Expense> ExpenseList = new List<Expense>();

        public IActionResult Index()
        {
            return View();
        }

        //add new expense to list
        public void AddExpenseToList(Expense expense)
        {
            ExpenseList.Add(expense);
        }

        //need function to sort list by expense date

        //method that fills the list with all expenses in the database then calls the sorting function



    }
}