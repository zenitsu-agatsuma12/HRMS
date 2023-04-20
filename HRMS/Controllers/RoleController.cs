using HRMS.Models;
using HRMS.Repository;
using HRMS.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class RoleController : Controller
    {
        IDepartmentPositionRepository _repo;
        private UserManager<ApplicationUser> _userManager { get; }
        private SignInManager<ApplicationUser> _signInManager { get; }
        public RoleManager<IdentityRole> _roleManager { get; }

        public RoleController(UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signInManager,
                                RoleManager<IdentityRole> roleManager,
                                 IDepartmentPositionRepository repo )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _repo = repo;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole
                {
                    Name = roleViewModel.Name
                };
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("List");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(roleViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ListAsync()
        {
            ViewBag.Departments = _repo.GetDepartmentList();
            var usersWithRoles = new List<UserRoleViewModel>();
            var roles = await _roleManager.Roles.ToListAsync();

            foreach (var role in roles)
            {
                var users = await _userManager.GetUsersInRoleAsync(role.Name);
                var userList = users.Where(u => u.ActiveStatus == true)
                                    .Where(e => e.Email != "administrator@pjli.com");
                foreach (var user in userList)
                {
                    var userdetails= _userManager.Users.Include(d => d.Department).FirstOrDefault(e => e.Email == user.Email);
                    usersWithRoles.Add(new UserRoleViewModel
                    {
                        UserId = user.Id,
                        FullName = user.FullName,
                        Email = user.Email,
                        RoleName = role.Name,
                        Department = userdetails.Department.DeptName,
                        deptId = userdetails.Department.DeptId,
                    });
                }
            }

            return View(usersWithRoles);
        }

        [HttpGet]
        public async Task<IActionResult> Update(string roleId)
        {
            var oldTodo = await _roleManager.FindByIdAsync(roleId);
            return View(oldTodo);
        }
        [HttpPost]
        public async Task<IActionResult> Update(RoleViewModel role)
        {
            var oldRole = await _roleManager.FindByIdAsync(role.Id.ToString());
            oldRole.Name = role.Name;
            var result = await _roleManager.UpdateAsync(oldRole);
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

        public async Task<IActionResult> Delete(string roleId)
        {
            var oldRole = await _roleManager.FindByIdAsync(roleId);

            var todolist = _roleManager.DeleteAsync(oldRole);
            return RedirectToAction(controllerName: "Role", actionName: "List"); // reload the getall page it self
        }

        //Update User Role
        [HttpGet]
        public async Task<IActionResult> UpdateUserRoleAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var roleName = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            var viewModel = new UserRoleViewModel
            {
                UserId = user.Id,
                FullName = user.FullName,
                RoleName = roleName
            };

            ViewBag.Roles = _roleManager.Roles.Where(r => r.Name != "Administrator").ToList();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var currentRole = await _userManager.GetRolesAsync(user);
            

            var result = await _userManager.RemoveFromRolesAsync(user, currentRole);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Failed to remove user from current roles.");
                return BadRequest(ModelState);
            }

            result = await _userManager.AddToRoleAsync(user, roleName);

            if (!result.Succeeded)
            {
                TempData["RoleAlert"] ="Failed to update user to selected role.";
                ModelState.AddModelError(string.Empty, "Failed to add user to selected role.");
                return BadRequest(ModelState);
            }
            TempData["RoleAlert"] = user.FullName + " Role Updated Successfully!";
            return RedirectToAction("List");
        }
    }
}
