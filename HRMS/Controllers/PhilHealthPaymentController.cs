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
        public IActionResult List()
        {
            var list = _repo.ListOfPhilHealthPayment();
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
                _repo.AddPhilHealthPayment(philHealthPayment);
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
            return RedirectToAction("List");
        }
        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(int No)
        {
            _repo.DeletePhilHealthPayment(No);
            return RedirectToAction("List");
        }
    }
}
