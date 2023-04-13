using HRMS.Data;
using HRMS.Models;
using HRMS.Repository;
using HRMS.Repository.SqlRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Controllers
{
    public class EmployeePerformanceController : Controller
    {

        IEmployeePerformanceDBRepository _repo;
        private UserManager<ApplicationUser> _userManager { get; }
        public RoleManager<IdentityRole> _roleManager { get; }
        public EmployeePerformanceController(IEmployeePerformanceDBRepository repo, UserManager<ApplicationUser> userManager)
        {
            _repo = repo;
            _userManager = userManager;
        }

        public IActionResult List()
        {
            var email = User.Identity.Name;
            var employee = _userManager.Users.FirstOrDefault(x => x.Email == email);
            if (User.IsInRole("Administrator"))
            {
                var value = _repo.ListOfEmployeePerformance(null);
                foreach (var item in value)
                {
                    var fullName = _userManager.Users.FirstOrDefault(x => x.Id == item.userID);
                    item.userID = fullName.FullName;
                }
                return View(value);
            }
            
            else if (User.IsInRole("Manager"))
            {
                var value = _repo.ListOfEmployeePerformanceReviewBy(employee.FullName);

                if (value != null)
                {
                    foreach (var item in value)
                    {
                        var userReview = _userManager.Users.FirstOrDefault(e => e.Id == item.userID);
                        item.userID = userReview.FullName;
                    }
                    return View(value);
                }
            }
            return View();
        }
        public IActionResult ProfileList()
        {
            var email = User.Identity.Name;
            var employee = _userManager.Users.FirstOrDefault(x => x.Email == email);
            if (User.IsInRole("Employee"))
            {
                var value = _repo.ListOfEmployeePerformance(employee.Id);
                foreach (var item in value)
                {
                    item.userID = employee.FullName;
                }

                return View(value);
            }
            
            return View();
        }
        
        public IActionResult ManagerReviewList()
        {
            var email = User.Identity.Name;
            var manager = _userManager.Users.FirstOrDefault(x => x.Email == email);
            var value = _repo.ListOfEmployeePerformance(null).Where(r => r.ReviewBy == manager.FullName);  
            foreach (var item in value)
            {
                var employee = _userManager.Users.FirstOrDefault(e => e.Id == item.userID);
                item.userID = employee.FullName;
            }

            return View(value);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, Manager")]
        public async Task<IActionResult> CreateAsync(string employeeName, string userID)
        {

            var email = User.Identity.Name;
            var employee = _userManager.Users.FirstOrDefault(e => e.Email == email);
            EmployeePerformance employeePerformance = new EmployeePerformance();
            {
                string reviewerName = employee.FirstName + " " + employee.MiddleName + " " + employee.LastName;

                employeePerformance.userID = userID;
                //employeePerformance.EmployeeName = employeeName;
                employeePerformance.ReviewBy = reviewerName;
                employeePerformance.Status = true;
                return View(employeePerformance);
            }
        }
        [HttpPost]
        [Authorize(Roles = "Administrator, Manager")]
        public IActionResult Create(EmployeePerformance newEmployeePerformance)
        {

            if (ModelState.IsValid)
            {
                var Dept = _repo.AddEmployeePerformance(newEmployeePerformance);
                return RedirectToAction("List");
            }
            ViewData["Message"] = "Data is not valid to create the Department";
            return View();

        }

        public async Task<IActionResult> Unread(int No)
        {
            var employee = _repo.GetEmployeePerformanceById(No);
            {
                employee.Status = false;
            }

            var result = _repo.UpdateEmployeePerformance(No, employee);
            
           
            return RedirectToAction("ProfileList");
           
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, Manager")]
        public IActionResult Update(int no)
        {
            var employee = _repo.GetEmployeePerformanceById(no);
            return View(employee);
        }
        [HttpPost]
        [Authorize(Roles = "Administrator, Manager")]
        public IActionResult Update(int no,EmployeePerformance newPerformance)
        {
            var result = _repo.UpdateEmployeePerformance(no,newPerformance);
            return RedirectToAction("List");
        }
        [Authorize(Roles = "Administrator, Manager")]
        public IActionResult Delete(int no)
        {
            _repo.DeleteEmployeePerformance(no);
            return RedirectToAction("List");

        }
        public IActionResult Details(int no)
        {
            return View(_repo.GetEmployeePerformanceById(no));
        }
    }
}
