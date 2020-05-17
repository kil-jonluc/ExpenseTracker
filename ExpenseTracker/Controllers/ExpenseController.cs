using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Helpers;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ExpenseTracker.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IConfiguration _configuration;

        public ExpenseController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _accessor = httpContextAccessor;
            _configuration = configuration;
        }

        // GET: Expense
        public ActionResult Index()
        {
            // In the future, pass the employer's id to get the list of expenses for the employer
            ExpenseDB expenseDB = new ExpenseDB(_configuration);
            var expenses = expenseDB.GetExpensesByEmployer(1);
            return View(expenses);
        }

        // GET: Expense/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Expense/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Expense/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Expense expense)
        {
            try
            {
                // Currently, hard code all expenses as belonging to company 1
                expense.EmployerId = 1;
                ExpenseDB expenseDB = new ExpenseDB(_configuration);
                expenseDB.InsertExpense(expense);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(expense);
            }
        }

        // GET: Expense/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Expense/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Expense/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Expense/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}