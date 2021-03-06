﻿using System;
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
            // If employer look up by employer ID that matched user ID if employee then look them up based on user ID
            ExpenseDB expenseDB = new ExpenseDB(_configuration);
            UserDB userDB = new UserDB(_configuration);
            var user = _accessor.HttpContext.Session.GetObjectFromJson<User>("LoggedInUser");
            ViewBag.Type = user.RoleId;
            IEnumerable<Expense> expenses;
            if (user.RoleId == 0)
            {
                expenses = expenseDB.GetExpensesByEmployer(user.EmployerId);
                foreach (var expense in expenses)
                {
                    if (expense.EmployeeName == " ")
                    {
                        var tempUser = userDB.GetUserById(expense.UserID);
                        expense.EmployeeName = tempUser.FirstName + " " + tempUser.LastName;
                    }
                }
            }
            else
            {
                expenses = expenseDB.GetExpenseByUserID(user.IDNumber);

            }
            return View(expenses);
        }

        // GET: Expense/Details/5
        public ActionResult Details(int id)
        {
            ExpenseDB expenseDb = new ExpenseDB(_configuration);
            return View(expenseDb.GetExpenseById(id));
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
                User user = _accessor.HttpContext.Session.GetObjectFromJson<User>("LoggedInUser");

                // sanity check, should never by false
                if (user != null)
                {
                    // Currently, hard code all expenses as belonging to company 1
                    // Need the expense to be connected to the same employer as the employee creating the expense
                    // TODO: Add employer to User model
                    expense.EmployerId = user.EmployerId;
                    ExpenseDB expenseDB = new ExpenseDB(_configuration);
                    expenseDB.InsertExpense(expense, user.IDNumber);
                }
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
            ExpenseDB expenseDb = new ExpenseDB(_configuration);
            User user = _accessor.HttpContext.Session.GetObjectFromJson<User>("LoggedInUser");
            ViewBag.Type = user.RoleId;
            return View(expenseDb.GetExpenseById(id));
        }

        // POST: Expense/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Expense expense)
        {
            try
            {
                ExpenseDB expenseDB = new ExpenseDB(_configuration);
                User user = _accessor.HttpContext.Session.GetObjectFromJson<User>("LoggedInUser");

                if (user.RoleId == 0)
                {
                    expenseDB.UpdateExpenseStatus(expense);
                }
                else
                {
                    expenseDB.UpdateExpense(expense);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View(expense);
            }
        }

        // GET: Expense/Delete/5
        public ActionResult Delete(int id)
        {
            ExpenseDB expenseDb = new ExpenseDB(_configuration);
            return View(expenseDb.GetExpenseById(id));
        }

        // POST: Expense/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                ExpenseDB expenseDb = new ExpenseDB(_configuration);
                expenseDb.DeleteExpense(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}