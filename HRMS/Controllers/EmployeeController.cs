using HRMS.Data;
using HRMS.Models;
using HRMS.Repository;
using HRMS.Repository.SqlRepository;
using HRMS.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Controllers
{
    //  [Authorize (Roles = "Human Resource,Employee")]
    public class EmployeeController : Controller
    {
        IEmployeeRepository _repo;
        private UserManager<ApplicationUser> _userManager { get; }
        public RoleManager<IdentityRole> _roleManager { get; }
        private SignInManager<ApplicationUser> _signInManager { get; }
        public EmployeeController(UserManager<ApplicationUser> userManager, IEmployeeRepository repo, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _repo = repo;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        //Get All the Employee




        public async Task<IActionResult> List(int searchOption = 0, string employeeSearch = "")
        {
            var employees = _userManager.Users.Include(d => d.Department).Where(status => status.ActiveStatus == true).Include(p => p.Position).ToList();
            if (searchOption > 0)
            {
                employees = _userManager.Users.Where(status => status.Department.DeptId == searchOption).ToList();
            }

            if (!string.IsNullOrEmpty(employeeSearch))
            {

                employees = _userManager.Users.Where(e => e.FullName.Contains(employeeSearch)).ToList();
            }

            ViewBag.Departments = _repo.GetDepartmentList();

            return View(employees.ToList());
        }

        public async Task<IActionResult> InactiveList(int searchOption = 0, string employeeSearch = "")
        {
            var inaciveCount = await _userManager.Users.Where(status => status.ActiveStatus==false).CountAsync();
            ViewBag.NumberOfInActive = inaciveCount;
            var employees = _userManager.Users.Include(d => d.Department).Where(status => status.ActiveStatus == false).Include(p => p.Position).ToList();
            if (searchOption > 0)
            {
                employees = _userManager.Users.Where(status => status.Department.DeptId == searchOption).ToList();
            }

            if (!string.IsNullOrEmpty(employeeSearch))
            {

                employees = _userManager.Users.Where(e => e.FullName.Contains(employeeSearch)).ToList();
            }

            ViewBag.Departments = _repo.GetDepartmentList();

            return View(employees.ToList());
        }

        public IActionResult Details(string accountId)
        {
            ViewBag.DepartmentList = _repo.GetDepartmentList();
            ViewBag.PositionList = _repo.GetPositionList();
            var employee = _userManager.Users.Include(d => d.Department).Include(p => p.Position).FirstOrDefault(u => u.Id == accountId);
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

        //Update Accout
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
                return RedirectToAction("List");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();

        }

        //Drop Delete Employee
        public async Task<IActionResult> Delete(string accountId)
        {
            var oldValue = await _userManager.FindByIdAsync(accountId);
            {
                oldValue.ActiveStatus = false;
            }

            var result = await _userManager.UpdateAsync(oldValue);
            if (result.Succeeded)
            {
                return RedirectToAction("List");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }
        public async Task<IActionResult> DeleteFormInActive(string accountId)
        {
            var oldValue = await _userManager.FindByIdAsync(accountId);
            {
                oldValue.ActiveStatus = true;
            }

            var result = await _userManager.UpdateAsync(oldValue);
            if (result.Succeeded)
            {
                return RedirectToAction("List");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }

        //Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.DepartmentList = _repo.GetDepartmentList();
            ViewBag.PositionList = _repo.GetPositionList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RegisterEmployeeViewModel employeeViewModel)
        {
            if (ModelState.IsValid)
            {
                var employeeModel = new ApplicationUser
                {
                    Email = employeeViewModel.Email,
                    UserName = employeeViewModel.Email,
                    FirstName = employeeViewModel.FirstName,
                    MiddleName = employeeViewModel.MiddleName,
                    LastName = employeeViewModel.LastName,
                    FullName = employeeViewModel.FirstName + " " + employeeViewModel.MiddleName + " " + employeeViewModel.LastName,
                    Gender = employeeViewModel.Gender,
                    DateOfBirth = employeeViewModel.DateOfBirth,
                    Phone = employeeViewModel.Phone,
                    DepartmentId = employeeViewModel.DepartmentId,
                    EmployeeType = employeeViewModel.EmployeeType,
                    SSSNumber = employeeViewModel.SSSNumber,
                    PhilHealthId = employeeViewModel.PhilHealthId,
                    PagIbigId = employeeViewModel.PagIbigId,
                    Street = employeeViewModel.Street,
                    Barangay = employeeViewModel.Barangay,
                    City = employeeViewModel.City,
                    State = employeeViewModel.State,
                    PostalCode = employeeViewModel.PostalCode,
                    DateHired = employeeViewModel.DateHired,
                    ActiveStatus = true,
                };
                var result = await _userManager.CreateAsync(employeeModel, employeeViewModel.Password);
                if (result.Succeeded)
                {
                    // add roles to it and allow him to login
                    var role = _roleManager.Roles.FirstOrDefault(r => r.Name == "Employee");
                    if (role != null)
                    {
                        var roleResult = await _userManager.AddToRoleAsync(employeeModel, role.Name);
                        if (!roleResult.Succeeded)
                        {
                            ModelState.AddModelError(String.Empty, "employee Role cannot be assigned");
                        }
                    }

                    // login the employee automatically
                    await _signInManager.SignInAsync(employeeModel, isPersistent: false);
                    return RedirectToAction("List", "Employee");

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(employeeViewModel);
        }

        // Create Pefromance Review

    }
}

