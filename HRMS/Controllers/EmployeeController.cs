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
        IDepartmentPositionRepository _departmentPositionRepository;
        private UserManager<ApplicationUser> _userManager { get; }
        public RoleManager<IdentityRole> _roleManager { get; }
        private SignInManager<ApplicationUser> _signInManager { get; }
        public EmployeeController(UserManager<ApplicationUser> userManager, 
                                   RoleManager<IdentityRole> roleManager, 
                                   SignInManager<ApplicationUser> signInManager,
                                   IEmployeeRepository repo,
                                   IDepartmentPositionRepository departmentPositionRepository)
        {
            _userManager = userManager;
            _repo = repo;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _departmentPositionRepository = departmentPositionRepository;
        }

        //Get All the Employee
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> List()
        {
            var employees = _userManager.Users.Include(d => d.Department)
                                              .Include(p => p.Position)
                                              .ToList()
                                              .Where(s => s.ActiveStatus == true && 
                                              _userManager.GetRolesAsync(s).Result.Contains("Employee") || 
                                              _userManager.GetRolesAsync(s).Result.Contains("Manager"))
                                              .ToList();

            ViewBag.Departments = _repo.GetDepartmentList();

            return View(employees.ToList());
        }

        //Return the Inactive Status
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> InactiveList()
        {
            var inaciveCount = await _userManager.Users.Where(status => status.ActiveStatus==false)
                                                       .Where(delete => delete.DeleteStatus==false)
                                                       .CountAsync();    

            var employees = _userManager.Users.Include(d => d.Department)
                                              .Where(status => status.ActiveStatus == false)
                                              .Where(delete => delete.DeleteStatus == false)
                                              .Include(p => p.Position).ToList();

            ViewBag.NumberOfInActive = inaciveCount;
            ViewBag.Departments = _repo.GetDepartmentList();

            return View(employees.ToList());
        }

        // Return the Deatails
        [Authorize(Roles = "Administrator, Employee, Manager")]
        public IActionResult Details(string accountId)
        {
            ViewBag.DepartmentList = _departmentPositionRepository.GetDepartmentList();
            ViewBag.PositionList = _departmentPositionRepository.GetPosition();
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

        //Update Account
        [Authorize(Roles = "Administrator, Employee, Manager")]
        [HttpGet]
        public async Task<IActionResult> Update(string accountId)
        {
            var employee = _userManager.Users.Include(d => d.Department).Include(p => p.Position).FirstOrDefault(u => u.Id == accountId);
            ViewBag.DepartmentList = _departmentPositionRepository.GetDepartmentList();
            ViewBag.PositionList = _departmentPositionRepository.GetPosition();
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

        [Authorize(Roles = "Administrator, Employee, Manager")]
        [HttpPost]
        public async Task<IActionResult> Update(EditEmployeeViewModel employee)
        {
            ViewBag.DepartmentList = _departmentPositionRepository.GetDepartmentList();
            ViewBag.PositionList = _departmentPositionRepository.GetPosition();
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
                    if (User.IsInRole("Administrator"))
                    {
                        TempData["EmployeeAlert"] = oldValue.FullName + " Details is Successfully Updated!";
                        return RedirectToAction("List");
                    }
                    else
                    {
                        return RedirectToAction("Profile", "Details");
                    }

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();

        }


        //Drop Delete Employee
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(string accountId)
        {
            var oldValue = await _userManager.FindByIdAsync(accountId);
            {
                oldValue.DeleteStatus = true;
            }

            var result = await _userManager.UpdateAsync(oldValue);
            if (result.Succeeded)
            {
                TempData["EmployeeAlert"] = oldValue.FullName + " is Successfully Deleted in the Record!";
                return RedirectToAction("List");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }

        // Update the Active status to false
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteFromActive(string accountId)
        {
            var oldValue = await _userManager.FindByIdAsync(accountId);
            {
                oldValue.ActiveStatus = false;
            }

            var result = await _userManager.UpdateAsync(oldValue);
            if (result.Succeeded)
            {
                TempData["EmployeeAlert"] = oldValue.FullName + " Account is Now Deactivated!";
                return RedirectToAction("List");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }

        // Update the active status to false
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteFormInActive(string accountId)
        {
            var oldValue = await _userManager.FindByIdAsync(accountId);
            {
                TempData["EmployeeAlert"] = oldValue.FullName + " Account is Now Activated!";
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
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.DepartmentList = _departmentPositionRepository.GetDepartmentList();
            ViewBag.PositionList = _departmentPositionRepository.GetPosition();
            return View();
        }
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Create(RegisterEmployeeViewModel employeeViewModel)
        {
            ViewBag.DepartmentList = _departmentPositionRepository.GetDepartmentList();
            ViewBag.PositionList = _departmentPositionRepository.GetPosition();
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
                    PositionId = employeeViewModel.PositionId,
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
                    TempData["EmployeeAlert"] = "New Account is Successfully Added!";
                    return RedirectToAction("List", "Employee");

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(employeeViewModel);
        }

        // Employe Performance Review
        public async Task<IActionResult> DepartamentalList()
        {
            var email = User.Identity.Name;
            var manager = await _userManager.GetUsersInRoleAsync("Manager");
            

            var employee = _userManager.Users.Include(d => d.Department)
                                             .FirstOrDefault(e => e.Email == email);

            var managerName = manager.Where(d => d.DepartmentId == employee.DepartmentId).ToList();

            var employeeList = _userManager.Users.Include(d => d.Department)
                                                 .Include(p => p.Position)
                                                 .Where(status => status.ActiveStatus == true)
                                                 .Where(e => e.DepartmentId == employee.DepartmentId)
                                                 .ToList() 
                                                 .Where(e => !_userManager.IsInRoleAsync(e, "Manager").Result)
                                                 .Where(e => !_userManager.IsInRoleAsync(e, "Administrator").Result)
                                                 .ToList();


            if (User.IsInRole("Manager"))
            {
                ViewBag.DepartmentHead = manager.Where(d => d.DepartmentId == employee.DepartmentId).Select(f => f.FullName).ToList();
            }
            else
            {
                
                if (managerName == null)
                {
                    ViewBag.DepartmentHead = "Unassigned";
                }
                else
                {
                    //ViewBag.DepartmentHead = managerName.FullName;
                    ViewBag.DepartmentHead = manager.Where(d => d.DepartmentId == employee.DepartmentId).ToList();
                }      
            }

            ViewBag.DepartmentName = employee.Department.DeptName;
            return View(employeeList.ToList());   
        }

    }
}

