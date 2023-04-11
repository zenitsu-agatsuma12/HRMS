using HRMS.Data;
using HRMS.Models;
using HRMS.Repository;
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

        public IActionResult List()
        {
            var list = _repo.ListOfSSSPayment();
            return View(list);
        }
        [HttpGet]
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
        public IActionResult Create(SSSPayment sssPayment)
            { 
                if (ModelState.IsValid)
                {
                    _repo.AddSSSPayment(sssPayment);
                    return RedirectToAction("List");
                }
                return View();
            }
        [HttpGet]
        public IActionResult Edit(int No)
        {
            SSSPayment ssspayment = _repo.GetSSSPaymentById(No);
            return View(ssspayment);
        }

        [HttpPost]
        public IActionResult Edit(SSSPayment newSSSPayment)
        {
            _repo.UpdateSSSPayment(newSSSPayment);
            return RedirectToAction("List");
        }

        public IActionResult Delete(int No)
        {
            _repo.DeleteSSSPayment(No);
            return RedirectToAction("List");
        }

        public IActionResult Details(int No)
        {

            return View(_repo.GetSSSPaymentById(No));
        }
    }
}
