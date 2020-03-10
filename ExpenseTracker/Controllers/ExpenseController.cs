using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    public class ExpenseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // GET: User/Create
        public IActionResult NewExpense()
        {
            return View("NewExpense", new Expense());
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NewExpense(Expense expense)
        {

            //sends the newly created expense to a list
            //AddExpenseToList(expense);

            //need to add new expense to the data base 
            AddExpenseToDatabase(expense);
            //return to the dashboard
            return RedirectToAction("Dashboard", "User");
        }

        // need a method to edit and expense when an expenses is clicked on it pulls that item from the expense list 
        //then it finds the old one in the data base and updates it and then it updates the list from the database





        //add new expense to list
        public void AddExpenseToList(Expense expense)
        {
            List<Expense> ExpenseList = new List<Expense>();
            ExpenseList.Add(expense);
        }

        public void AddExpenseToDatabase(Expense expense)
        {

        }

        //need function to sort list by expense date

        //method that fills the list with all expenses in the database then calls the sorting function
    }
}