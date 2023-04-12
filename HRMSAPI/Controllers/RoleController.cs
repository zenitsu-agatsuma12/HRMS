using HRMSAPI.DTO;
using HRMSAPI.Models;
using HRMSAPI.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMSAPI.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public RoleController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Get All Roles
        [HttpGet]
        public async Task<IActionResult> GetAllRoleAsync()
        {
            var userwithroles = new List<UserRoleDTO>();
            var rolelist = await _roleManager.Roles.ToListAsync();
            foreach (var role in rolelist)
            {
                var userlist = await _userManager.GetUsersInRoleAsync(role.Name);
                foreach (var user in userlist)
                {
                    userwithroles.Add(new UserRoleDTO
                    {
                        UserId = user.Id,
                        FullName = user.FirstName + " " + user.MiddleName + " " + user.LastName,
                        RoleName = role.Name
                    });
                }
            }
            return Ok(userwithroles);
        }

        // Get Role By Id
        [HttpGet("{id}")]

        public async Task<IActionResult> GetRoleById([FromRoute]string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var userFullName = user.FullName;
            var userwithrole = await _userManager.GetRolesAsync(user);
            return Ok(new { Id = id, FullName = userFullName, Roles = userwithrole });
        }

        //Update User Role
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserRoleAsync([FromBody] EditUserRoleDTO editUserRoleDTO, [FromRoute] string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);

            // Remove the user from all the current roles
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                return BadRequest("Failed to remove user from current roles");
            }

            // Add the user to the new role
            var role = await _roleManager.FindByNameAsync(editUserRoleDTO.RoleName);
            if (role == null)
            {
                return BadRequest("Role does not exist");
            }
            result = await _userManager.AddToRoleAsync(user, role.Name);
            if (!result.Succeeded)
            {
                return BadRequest("Failed to add user to the new role");
            }
            return Ok(new { message = "User role updated successfully" });
        }

    }
}
