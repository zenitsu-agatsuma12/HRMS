using HRMS.Models;
using HRMS.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class DashboardController : Controller
    {
        IDepartmentRepository _department;
        IPositionRepository _position;
        IEmployeePerformanceDBRepository _employeePerformance;
        private UserManager<ApplicationUser> _userManager { get; }
        public RoleManager<IdentityRole> _roleManager { get; }
        private SignInManager<ApplicationUser> _signInManager { get; }
        public DashboardController(UserManager<ApplicationUser> userManager,  
                                  RoleManager<IdentityRole> roleManager,
                                  SignInManager<ApplicationUser> signInManager,
                                  IDepartmentRepository repo,
                                  IPositionRepository position,
                                  IEmployeePerformanceDBRepository employeePerformance)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _position = position;
            _department = repo;
            _employeePerformance = employeePerformance;
        }
        public IActionResult Index()
        {
            var employees = _userManager.Users.Where(status => status.ActiveStatus == false).Where(d=>d.DeleteStatus==false).ToList();
            ViewBag.Employees = _userManager.Users.Where(status => status.ActiveStatus == true).Count()-1;
            ViewBag.Departments = _department.ListOfDepartment().Count();
            ViewBag.Positions = _position.ListOfPosition().Count();
            ViewBag.EmployeeInActive = employees;
            ViewBag.EmployeePerformance = _employeePerformance.ListOfEmployeePerformance(null).Where(e => e.Status == true);
            return View();
        }
    }
}
