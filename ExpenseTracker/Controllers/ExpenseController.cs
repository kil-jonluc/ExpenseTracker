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
            return RedirectToAction("Index", "Dashboard");
        }
    }
}