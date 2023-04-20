using HRMS.Data;
using HRMS.Models;
using HRMS.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Intrinsics.X86;

namespace HRMS.Controllers
{

    public class SSSPaymentController : Controller
    {
        ISSSPaymentRepository _repo;
        private UserManager<ApplicationUser> _userManager { get; }
        public RoleManager<IdentityRole> _roleManager { get; }
        public SSSPaymentController(ISSSPaymentRepository repo, UserManager<ApplicationUser> userManager)
        {
            this._repo = repo;
                this._userManager = userManager;
        }

        [Authorize(Roles = "Administrator, Employee, Manager")]
        public IActionResult List(string searchValue)
        {
            if (!User.IsInRole("Administrator"))
            {
                var email = User.Identity.Name;
                var employee = _userManager.Users.FirstOrDefault(e => e.Email == email);
                var employeeList = _repo.ListOfSSSPayment(searchValue).Where(e => e.FullName == employee.FullName);
                return View(employeeList);
            }
            var list = _repo.ListOfSSSPayment(searchValue);
            return View(list);
        }
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult Create(string employeeName, string sss) 
        {
            var email = User.Identity.Name;
            var employee = _userManager.Users.FirstOrDefault(e => e.Email == email);
            SSSPayment newSSSPayment = new SSSPayment();
            {
                newSSSPayment.FullName = employeeName;
                newSSSPayment.SSSNumber= sss;
                newSSSPayment.Month = DateTime.Now.ToString("MMMM");
                newSSSPayment.Year = DateTime.Now.ToString("yyyy");
                return View(newSSSPayment);
            }

        }
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Create(SSSPayment sssPayment)
            { 
                if (ModelState.IsValid)
                {
                    var payment = _repo.AddSSSPayment(sssPayment);
                    TempData["SSSAlert"] ="Payment is Added to " +payment.FullName+ " Account";
                    return RedirectToAction("List");
                }
                return View();
            }
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(int No)
        {
            SSSPayment ssspayment = _repo.GetSSSPaymentById(No);
            return View(ssspayment);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(SSSPayment newSSSPayment)
        {
            _repo.UpdateSSSPayment(newSSSPayment);
            TempData["SSSAlert"] = "Payment is Updated Successfully";
            return RedirectToAction("List");
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(int No)
        {
            _repo.DeleteSSSPayment(No);
            TempData["SSSAlert"] = "Payment is Deleted Successfully";
            return RedirectToAction("List");
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Details(int No)
        {

            return View(_repo.GetSSSPaymentById(No));
        }
    }
}
