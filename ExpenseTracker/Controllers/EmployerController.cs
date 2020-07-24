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
    public class EmployerController : Controller
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IConfiguration _configuration;
        private readonly EmployerDB _employerDB;
        public EmployerController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _accessor = httpContextAccessor;
            _configuration = configuration;
            _employerDB = new EmployerDB(_configuration);
        }
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            return View(_employerDB.GetEmployerById(Id));
        }

        [HttpPost]
        public  IActionResult Edit(Employer employer)
        {
            // Validate Employer
            if (ModelState.IsValid)
            {
                try
                {
                    _employerDB.UpdateEmployer(employer);
                    return RedirectToAction("Dashboard", "User");
                }
                catch(Exception ex)
                {
                }
            }
         
            return View(employer);
        }
    }
}