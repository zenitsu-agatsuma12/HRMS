using HRMS.Models;
using HRMS.Repository;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Administrator, Employee, Manager")]
        public IActionResult List(string searchValue)
        {
            if (!User.IsInRole("Administrator"))
            {
                var email = User.Identity.Name;
                var employee = _userManager.Users.FirstOrDefault(e => e.Email == email);
                var employeeList = _repo.ListOfPagIbigPayment(searchValue).Where(e => e.FullName == employee.FullName);
                return View(employeeList);
            }
            var list = _repo.ListOfPagIbigPayment(searchValue);
            return View(list);
        }
        [HttpGet]
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
        public IActionResult Create(PagIbigPayment pagIbigPayment)
        {
            if (ModelState.IsValid)
            {
                var payment = _repo.AddPagIbigPayment(pagIbigPayment);
                TempData["PagibigAlert"] = "Payment is Added to " + payment.FullName + " Account";
                return RedirectToAction("List");
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(int No)
        {
            PagIbigPayment pagIbigPayment = _repo.GetPagIbigPaymentById(No);
            return View(pagIbigPayment);
        }
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(PagIbigPayment pagIbigPayment)
        {
            _repo.UpdatePagIbigPayment(pagIbigPayment);
            TempData["PagibigAlert"] = "Payment is Updated Successfully";
            return RedirectToAction("List");
        }
        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(int No)
        {
            _repo.DeletePagIbigPayment(No);
            TempData["PagibigAlert"] = "Payment is Deleted Successfully";
            return RedirectToAction("List");
        }
    }
}
