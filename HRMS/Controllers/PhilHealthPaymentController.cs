using HRMS.Models;
using HRMS.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
    public class PhilHealthPaymentController : Controller
    {
        IPhilHealthPaymentDBRepository _repo;
        private UserManager<ApplicationUser> _userManager { get; }

        public PhilHealthPaymentController(IPhilHealthPaymentDBRepository repo, UserManager<ApplicationUser> userManager)
        {
            _repo = repo;
            _userManager = userManager;
        }
        [Authorize(Roles = "Employee, Manager, Administrator")]
        public IActionResult List(string searchValue)
        {
            if (!User.IsInRole("Administrator"))
            {
                var email = User.Identity.Name;
                var employee = _userManager.Users.FirstOrDefault(e => e.Email == email);
                var employeeList = _repo.ListOfPhilHealthPayment(searchValue).Where(e => e.FullName == employee.FullName);
                return View(employeeList);
            }
            var list = _repo.ListOfPhilHealthPayment(searchValue);
            return View(list);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult Create(string employeeName, string philhealth)
        {
            var email = User.Identity.Name;
            var employee = _userManager.Users.FirstOrDefault(e => e.Email == email);
            PhilHealthPayment newPhilHealthPayment = new PhilHealthPayment();
            {
                newPhilHealthPayment.FullName = employeeName;
                newPhilHealthPayment.PhilHealthNumber = philhealth;
                newPhilHealthPayment.Month = DateTime.Now.ToString("MMMM");
                newPhilHealthPayment.Year = DateTime.Now.ToString("yyyy");
                return View(newPhilHealthPayment);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Create(PhilHealthPayment philHealthPayment)
        {
            if(ModelState.IsValid)
            {
                var payment = _repo.AddPhilHealthPayment(philHealthPayment);
                TempData["PhilhealthAlert"] = "Payment is Added to " + payment.FullName + " Account";
                return RedirectToAction("List");
            }
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(int No)
        {
            PhilHealthPayment philHealthPayment = _repo.GetPhilHealthPaymentById(No);
            return View(philHealthPayment);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(PhilHealthPayment philHealthPayment)
        {
            _repo.UpdatePhilHealthPayment(philHealthPayment);
            TempData["PhilhealthAlert"] = "Payment is Updated Successfully";
            return RedirectToAction("List");
        }
        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(int No)
        {
            _repo.DeletePhilHealthPayment(No);
            TempData["PhilhealthAlert"] = "Payment is Deleted Successfully";
            return RedirectToAction("List");
        }
    }
}
