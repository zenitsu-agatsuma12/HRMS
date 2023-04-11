using HRMS.Models;
using HRMS.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Controllers
{
  //  [Authorize(Roles = "Human Resource")]
    public class RoleController : Controller
    {
        private UserManager<ApplicationUser> _userManager { get; }
        // login user details 
        private SignInManager<ApplicationUser> _signInManager { get; }
        public RoleManager<IdentityRole> _roleManager { get; }

        public RoleController(UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signInManager,
                                RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Create()
        {

            //_roleManager.Roles;
            //_roleManager.DeleteAsync();
            //_roleManager.UpdateAsync();
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
            var usersWithRoles = new List<UserRoleViewModel>();

            var roles = await _roleManager.Roles.ToListAsync();

            foreach (var role in roles)
            {
                var users = await _userManager.GetUsersInRoleAsync(role.Name);

                foreach (var user in users)
                {
                    usersWithRoles.Add(new UserRoleViewModel
                    {
                        UserId = user.Id,
                        FullName = user.FullName,
                        RoleName = role.Name
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

            ViewBag.Roles = _roleManager.Roles.ToList();

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
                ModelState.AddModelError(string.Empty, "Failed to add user to selected role.");
                return BadRequest(ModelState);
            }

            return RedirectToAction("List");
        }
    }
}
