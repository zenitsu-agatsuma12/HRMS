using HRMS.Models;
using HRMS.Repository;
using HRMS.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRMS.Controllers
{
    public class AccountController : Controller
    {
        IEmployeeRepository _repo;
        IDepartmentPositionRepository _departmentPositionRepository;
        private UserManager<ApplicationUser> _userManager { get; }
        // login user details 
        private SignInManager<ApplicationUser> _signInManager { get; }
        public RoleManager<IdentityRole> _roleManager { get; }

        public AccountController(UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signInManager,
                                RoleManager<IdentityRole> roleManager,
                                IEmployeeRepository repo,
                                IDepartmentPositionRepository departmentPositionRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _repo = repo;
            _departmentPositionRepository = departmentPositionRepository;
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.DepartmentList = _departmentPositionRepository.GetDepartmentList();
            ViewBag.PositionList = _departmentPositionRepository.GetPosition();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterEmployeeViewModel employeeViewModel)
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
                    FullName = employeeViewModel.FirstName +" "+ employeeViewModel.MiddleName +" "+ employeeViewModel.LastName,
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
                    ActiveStatus = false,
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
                            ModelState.AddModelError(String.Empty, "Employee Role cannot be Assigned");
                        }
                    }

                    // login the employee automatically
                    await _signInManager.SignInAsync(employeeModel, isPersistent: false);
                    var statusCheck = _userManager.Users.FirstOrDefault(u => u.Email == employeeViewModel.Email);
                    if (statusCheck.ActiveStatus == false)
                    {
                        return RedirectToAction("Privacy", "Home");
                    }
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(employeeViewModel);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LogInEmployeeViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(userViewModel.UserName, userViewModel.Password, false, false);
   
                if (result.Succeeded)
                {
                    var statusCheck = _userManager.Users.FirstOrDefault(u => u.Email == userViewModel.UserName);
                    if (statusCheck.ActiveStatus == true )
                    {
                        var roles = await _userManager.GetRolesAsync(statusCheck);

                        if (roles.Contains("Administrator"))
                        {
                            return RedirectToAction("Index", "Dashboard");
                        }
                        return RedirectToAction("Details", "Profile");
                    }

                    return RedirectToAction("Privacy", "Home");

                }
                ModelState.AddModelError(string.Empty, "Invalid Login Credentials");
            }
            return View(userViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            await _signInManager.RefreshSignInAsync(user);
            TempData["AlertMessage"] = "Your password has been changed.";
            return RedirectToAction("Details","Profile");
        }

        [HttpGet]
        public JsonResult GetPositionByDepartment(int departmentId)
        {
            List<SelectListItem> positionList = _departmentPositionRepository.GetPosition(departmentId);
            return Json(positionList);
        } 
    }
}
