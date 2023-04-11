﻿using HRMS.Models;
using HRMS.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
    public class PagIbigPaymentController : Controller
    {
        IPagIbigPaymentRepository _repo;
        private UserManager<ApplicationUser> _userManager { get; }
        public PagIbigPaymentController(IPagIbigPaymentRepository repo, UserManager<ApplicationUser> userManager)
        {
            _repo = repo;
            _userManager = userManager;
        }

        public IActionResult List()
        {
            var list = _repo.ListOfPagIbigPayment();
            return View(list);
        }
        [HttpGet]

        public IActionResult Create(string employeeName, string pagibig)
        {
            var email = User.Identity.Name;
            var employee = _userManager.Users.FirstOrDefault(e => e.Email == email);
            PagIbigPayment newSSSPayment = new PagIbigPayment();
            {
                newSSSPayment.FullName = employeeName;
                newSSSPayment.PagIbigNumber = pagibig;
                newSSSPayment.Month = DateTime.Now.ToString("MMMM");
                newSSSPayment.Year = DateTime.Now.ToString("yyyy");
                return View(newSSSPayment);
            }
        }
        [HttpPost]
        public IActionResult Create(PagIbigPayment pagIbigPayment)
        {
            if (ModelState.IsValid)
            {
                _repo.AddPagIbigPayment(pagIbigPayment);
                return RedirectToAction("List");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int No)
        {
            PagIbigPayment pagIbigPayment = _repo.GetPagIbigPaymentById(No);
            return View(pagIbigPayment);
        }
        [HttpPost]

        public IActionResult Edit(PagIbigPayment pagIbigPayment)
        {
            _repo.UpdatePagIbigPayment(pagIbigPayment);
            return RedirectToAction("List");
        }

        public IActionResult Delete(int No)
        {
            _repo.DeletePagIbigPayment(No);
            return RedirectToAction("List");
        }
    }
}