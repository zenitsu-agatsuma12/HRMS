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
        IDepartmentRepository _deptrepo;
        private UserManager<ApplicationUser> _userManager { get; }
        public RoleManager<IdentityRole> _roleManager { get; }
        public EmployeePerformanceController(IEmployeePerformanceDBRepository repo,
                                             IDepartmentRepository deptrepo, 
                                             UserManager<ApplicationUser> userManager)
        {
            _repo = repo;
            _userManager = userManager;
            _deptrepo = deptrepo;
        }

        public IActionResult List(string searchValue)
        {
            ViewBag.DepartmentList = _deptrepo.GetDepartmentList();
            var email = User.Identity.Name;
            var employee = _userManager.Users.FirstOrDefault(x => x.Email == email);
            if (User.IsInRole("Administrator"))
            {
                var value = _repo.SearchListOfEmployeePerformance(null, searchValue);
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

        //Return Profile List Review 
        public IActionResult ProfileList()
        {
            var email = User.Identity.Name;
            var employee = _userManager.Users.FirstOrDefault(x => x.Email == email);
            if(employee != null)
            {
                if (!User.IsInRole("Administrator"))
                {
                    var value = _repo.ListOfEmployeePerformance(employee.Id);
                    if (value != null)
                    {
                        foreach (var item in value)
                        {
                            item.userID = employee.FullName;
                        }

                        return View(value);
                    }
                    return View();
                }
                return View();
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

        //Create Peformance Review
        [HttpGet]
        [Authorize(Roles = "Administrator, Manager")]
        public async Task<IActionResult> CreateAsync(string employeeName, string userID)
        {
            var email = User.Identity.Name;
            var employee = _userManager.Users.FirstOrDefault(e => e.Email == email);
            EmployeePerformance employeePerformance = new EmployeePerformance();
            {
                string reviewerName = employee.FullName;

                employeePerformance.userID = userID;
                employeePerformance.DateReview = DateTime.Now.ToString("MM/dd/yyyy");
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
                var employee = _repo.AddEmployeePerformance(newEmployeePerformance);
                TempData["EmployeePerformanceAlert"] = "The performance review is successfully sent to " + employee.EmployeeName;
                return RedirectToAction("List");
            }
            ViewData["Message"] = "Data is Not valid to create the Department";
            return View();

        }

        //Update to Read
        public async Task<IActionResult> Unread(int No)
        {
            var employee = _repo.GetEmployeePerformanceById(No);
            {
                employee.Status = false;
            }

            var result = _repo.UpdateEmployeePerformance(No, employee);
           
            return RedirectToAction("ProfileList");
        }

        //Update Performance Review
        [HttpGet]
        [Authorize(Roles = "Administrator, Manager")]
        public IActionResult Update(int No)
        {
            var employee = _repo.GetEmployeePerformanceById(No);
            return View(employee);
        }
        [HttpPost]
        [Authorize(Roles = "Administrator, Manager")]
        public IActionResult Update(int No,EmployeePerformance newPerformance)
        {
            var result = _repo.UpdateEmployeePerformance(No,newPerformance);
            TempData["EmployeePerformanceAlert"] = "Performance Review Successfully Updated!";
            return RedirectToAction("List");
        }

        //Delete Reviews
        [Authorize(Roles = "Administrator, Manager")]
        public IActionResult Delete(int No)
        {
            var employee = _repo.GetEmployeePerformanceById(No);
            {
                employee.DeleteStatus = true;
            }

            var result = _repo.UpdateEmployeePerformance(No, employee);
            TempData["EmployeePerformanceAlert"] = "Performance Review Successfully Deleted!";

            return RedirectToAction("List");

        }

        //get the Details Employee Performances
        public IActionResult Details(int No)
        {
            return View(_repo.GetEmployeePerformanceById(No));
        }

    }
}
