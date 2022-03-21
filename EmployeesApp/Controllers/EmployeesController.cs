﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmployeesApp.Models;
using EmployeesApp.Contracts;
using System;
using Microsoft.AspNetCore.DataProtection;
using System.Threading;

namespace EmployeesApp.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeeRepository _repo;
        private readonly IDataProtector _protector;

        public EmployeesController(IEmployeeRepository repo, IDataProtectionProvider provider)
        {
            _repo = repo;
            _protector = provider.CreateProtector("EmployeesApp.EmployeesController");
        }


        public IActionResult Index()
        {
            bool testProtcted = false;
            var employees = _repo.GetAll();
            foreach (var emp in employees)
            {
                var stringId = emp.Id.ToString();
                emp.EncryptedId = _protector.Protect(stringId);
            }
            //return View(employees);

            //Previous code removed for the example clarity
            if (testProtcted)
            {
                var timeLimitedProtector = _protector.ToTimeLimitedDataProtector();
                var timeLimitedData = timeLimitedProtector.Protect("Test timed protector", lifetime: TimeSpan.FromSeconds(2));
                //just to test that this action works as long as life-time hasn't expired
                var timedUnprotectedData = timeLimitedProtector.Unprotect(timeLimitedData);
                Thread.Sleep(3000);
                var anotherTimedUnprotectTry = timeLimitedProtector.Unprotect(timeLimitedData);
            }
            return View(employees);


        }

        public IActionResult Details(string id)
        {
            var guid_id = Guid.Parse(_protector.Unprotect(id));
            var employee = _repo.GetEmployee(guid_id);
            return View(employee);

        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,AccountNumber,Age")] Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return View(employee);
            }

            _repo.CreateEmployee(employee);
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
