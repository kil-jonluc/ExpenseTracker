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
        SharedController _shared;
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
            if (_shared == null)
            {
                _shared = new SharedController();
            }
            //sends the newly created expense to a list
            _shared.AddExpenseToList(expense);

            //need to add new expense to the data base 

            //return to the dashboard
            return RedirectToAction("Index", "Dashboard");
        }

        // need a method to edit and expense when an expenses is clicked on it pulls that item from the expense list 
        //then it finds the old one in the data base and updates it and then it updates the list from the database
    }
}