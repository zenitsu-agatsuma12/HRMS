using HRMS.Models;
using HRMS.Repository;
using HRMS.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Controllers
{
    public class ProfileController : Controller
    {
        IEmployeeRepository _repo;
        private UserManager<ApplicationUser> _userManager { get; }
        public RoleManager<IdentityRole> _roleManager { get; }
        private SignInManager<ApplicationUser> _signInManager { get; }
        public ProfileController(UserManager<ApplicationUser> userManager, IEmployeeRepository repo, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _repo = repo;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> DetailsAsync()
        {
            ViewBag.DepartmentList = _repo.GetDepartmentList();
            ViewBag.PositionList = _repo.GetPositionList();
            
            var email = User.Identity.Name;
            var employee = _userManager.Users.Include(d => d.Department).Include(p => p.Position).FirstOrDefault(e => e.Email == email);
            var roles = await _userManager.GetRolesAsync(employee);

            // assuming the employee has only one role
            ViewBag.UserRole = roles.FirstOrDefault();

            EditEmployeeViewModel employeeViewModel = new EditEmployeeViewModel()
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                MiddleName = employee.MiddleName,
                LastName = employee.LastName,
                Department = employee.Department,
                Position = employee.Position,
                Gender = employee.Gender,
                DateOfBirth = employee.DateOfBirth,
                Email = employee.Email,
                Phone = employee.Phone,
                DepartmentId = employee.DepartmentId,
                PositionId = employee.PositionId,
                EmployeeType = employee.EmployeeType,
                SSSNumber = employee.SSSNumber,
                PhilHealthId = employee.PhilHealthId,
                PagIbigId = employee.PagIbigId,
                Street = employee.Street,
                Barangay = employee.Barangay,
                City = employee.City,
                State = employee.State,
                PostalCode = employee.PostalCode,
                DateHired = employee.DateHired,
                ActiveStatus = employee.ActiveStatus,
            };
            return View(employeeViewModel);

        }

        //Update
        [HttpGet]
        public async Task<IActionResult> Update(string accountId)
        {
            var employee = _userManager.Users.Include(d => d.Department).Include(p => p.Position).FirstOrDefault(u => u.Id == accountId);
            ViewBag.DepartmentList = _repo.GetDepartmentList();
            ViewBag.PositionList = _repo.GetPositionList();
            //  var roles = await _userManager.GetRolesAsync(user);
            EditEmployeeViewModel employeeViewModel = new EditEmployeeViewModel()
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                MiddleName = employee.MiddleName,
                LastName = employee.LastName,
                Gender = employee.Gender,
                DateOfBirth = employee.DateOfBirth,
                Phone = employee.Phone,
                DepartmentId = employee.DepartmentId,
                PositionId = employee.PositionId,
                EmployeeType = employee.EmployeeType,
                SSSNumber = employee.SSSNumber,
                PhilHealthId = employee.PhilHealthId,
                PagIbigId = employee.PagIbigId,
                Street = employee.Street,
                Barangay = employee.Barangay,
                City = employee.City,
                State = employee.State,
                PostalCode = employee.PostalCode,
                DateHired = employee.DateHired,
                //    ActiveStatus = employee.ActiveStatus,
            };
            return View(employeeViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Update(EditEmployeeViewModel employee)
        {
            ViewBag.DepartmentList = _repo.GetDepartmentList();
            ViewBag.PositionList = _repo.GetPositionList();
            if (ModelState.IsValid)
            {
                var oldValue = await _userManager.FindByIdAsync(employee.Id.ToString());
                {
                    oldValue.FirstName = employee.FirstName;
                    oldValue.MiddleName = employee.MiddleName;
                    oldValue.LastName = employee.LastName;
                    oldValue.FullName = employee.FirstName + " " + employee.MiddleName + " " + employee.LastName;
                    oldValue.Gender = employee.Gender;
                    oldValue.DateOfBirth = employee.DateOfBirth;
                    oldValue.Phone = employee.Phone;
                    oldValue.DepartmentId = employee.DepartmentId;
                    oldValue.PositionId = employee.PositionId;
                    oldValue.EmployeeType = employee.EmployeeType;
                    oldValue.SSSNumber = employee.SSSNumber;
                    oldValue.PhilHealthId = employee.PhilHealthId;
                    oldValue.PagIbigId = employee.PagIbigId;
                    oldValue.Street = employee.Street;
                    oldValue.Barangay = employee.Barangay;
                    oldValue.City = employee.City;
                    oldValue.State = employee.State;
                    oldValue.PostalCode = employee.PostalCode;
                    oldValue.DateHired = employee.DateHired;
                    //   oldValue.ActiveStatus = employee.ActiveStatus;
                }

                var result = await _userManager.UpdateAsync(oldValue);
                if (result.Succeeded)
                {
                    return RedirectToAction("Details");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();

        }
    }
}
