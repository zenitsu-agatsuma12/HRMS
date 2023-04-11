using HRMS.Models;
using HRMS.Repository;
using HRMS.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
    public class AccountController : Controller
    {
        IEmployeeRepository _repo;
        private UserManager<ApplicationUser> _userManager { get; }
        // login user details 
        private SignInManager<ApplicationUser> _signInManager { get; }
        public RoleManager<IdentityRole> _roleManager { get; }

        public AccountController(UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signInManager,
                                RoleManager<IdentityRole> roleManager,
                                IEmployeeRepository repo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _repo = repo;
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.DepartmentList = _repo.GetDepartmentList();
            ViewBag.PositionList = _repo.GetPositionList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterEmployeeViewModel employeeViewModel)
        {
            ViewBag.DepartmentList = _repo.GetDepartmentList();
            ViewBag.PositionList = _repo.GetPositionList();
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
                            ModelState.AddModelError(String.Empty, "employee Role cannot be assigned");
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
                // login activity -> cookie [Roles and Claims]
                var result = await _signInManager.PasswordSignInAsync(userViewModel.UserName, userViewModel.Password, userViewModel.RememberMe, false);
                //login cookie and transfter to the client 
                if (result.Succeeded)
                {
                    var statusCheck = _userManager.Users.FirstOrDefault(u => u.Email == userViewModel.UserName);
                    if (statusCheck.ActiveStatus == true )
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    return RedirectToAction("Privacy", "Home");

                }
                ModelState.AddModelError(string.Empty, "invalid login credentials");
            }
            return View(userViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

       


    }
}
