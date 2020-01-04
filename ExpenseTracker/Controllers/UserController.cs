using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public IActionResult Index()
        {
            return View();
        }

        // GET: User/Create
        public IActionResult CreateUser()
        {
            return View("CreateUser", new User());
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUser(User user)
        {
            try
            {
                bool isError = false;
                if (user.FirstName != "Jon")
                {
                    ModelState.AddModelError("FirstName", "First Name Must Be Jon");
                    isError = true;
                }
                if (user.SSN == null)
                {
                    ModelState.AddModelError("SSN", "SSN must be significant");
                    isError = true;
                }
                if (isError)
                {
                    return View("CreateUser", user);
                }
                else
                {
                    ViewBag.message = "thanks";
                    return View("CreateSuccess");
                    //return RedirectToAction("Index", "Home");
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
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

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
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