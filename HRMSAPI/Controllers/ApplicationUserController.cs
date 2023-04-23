using AutoMapper;
using HRMSAPI.DTO;
using HRMSAPI.Models;
using HRMSAPI.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using System.ComponentModel;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace HRMSAPI.Controllers
{
    [Authorize(Roles = "Administrator")] 
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        IEmployeeRepository _repo;

        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public ApplicationUserController(IEmployeeRepository repo, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _repo = repo;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // List all employees
        [HttpGet]
        public IActionResult GetAll()
        {
            var employees = _userManager.Users.ToList();
            return Ok(employees);
        }

        //Get by Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var employee = await _userManager.FindByIdAsync(id);
            if (employee == null)
            {
                return BadRequest("No Resource Found!");
            }
            return Ok(employee);
        }


        // For Adding
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] AddApplicationUserDTO appDTO)
        {
            if (appDTO == null)
                BadRequest("No resource found");

            if (ModelState.IsValid)
            {
                var emp = new ApplicationUser()
                {
                    FirstName = appDTO.FirstName,
                    MiddleName = appDTO.MiddleName,
                    LastName = appDTO.LastName,
                    FullName = appDTO.FirstName + " " + appDTO.MiddleName + " " + appDTO.LastName,
                    Gender = appDTO.Gender,
                    DateOfBirth = appDTO.DateOfBirth,
                    Phone = appDTO.Phone,
                    Email = appDTO.Email,
                    UserName = appDTO.Email,
                    EmployeeType = appDTO.EmployeeType,
                    SSSNumber = appDTO.SSSNumber,
                    PagIbigId = appDTO.PagIbigId,
                    PhilHealthId = appDTO.PhilHealthId,
                    Street = appDTO.Street,
                    Barangay = appDTO.Barangay,
                    City = appDTO.City,
                    State = appDTO.State,
                    PostalCode = appDTO.PostalCode,
                    DateHired = appDTO.DateHired,
                    ActiveStatus = true,
                    DeleteStatus = false
                };

            var result = await _userManager.CreateAsync(emp, appDTO.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest(ModelState);
            }

            // add roles to it and allow him to login
            var role = _roleManager.Roles.FirstOrDefault(r => r.Name == "Employee");

            if (role != null)
            {
                var roleResult = await _userManager.AddToRoleAsync(emp, role.Name);

                if (!roleResult.Succeeded)
                {
                    foreach (var error in roleResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return BadRequest(ModelState);
                }
            }
            return Ok();
            }
            else
            {
            return BadRequest(ModelState);
        }

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] EditApplicationUserDTO editDTO)
        {
            var modeltoupdate = await _userManager.FindByIdAsync(id);
            if (modeltoupdate == null)
            {
                return NotFound();
            }
            modeltoupdate.Id = id;
            modeltoupdate.FirstName = editDTO.FirstName;
            modeltoupdate.MiddleName = editDTO.MiddleName;
            modeltoupdate.LastName = editDTO.LastName;
            modeltoupdate.FullName = editDTO.FirstName + " " + editDTO.MiddleName + " " + editDTO.LastName;
            modeltoupdate.Gender = editDTO.Gender;
            modeltoupdate.DateOfBirth = editDTO.DateOfBirth;
            modeltoupdate.Phone = editDTO.Phone;
            modeltoupdate.EmployeeType = editDTO.EmployeeType;
            modeltoupdate.SSSNumber = editDTO.SSSNumber;
            modeltoupdate.PagIbigId = editDTO.PagIbigId;
            modeltoupdate.PhilHealthId = editDTO.PhilHealthId;
            modeltoupdate.Street = editDTO.Street;
            modeltoupdate.Barangay = editDTO.Barangay;
            modeltoupdate.City = editDTO.City;
            modeltoupdate.State = editDTO.State;
            modeltoupdate.PostalCode = editDTO.PostalCode;
            modeltoupdate.DateHired = editDTO.DateHired;
            modeltoupdate.ActiveStatus = editDTO.ActiveStatus;
            var result = await _userManager.UpdateAsync(modeltoupdate);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest("No resource");
        }

        [HttpDelete]

        public async Task<IActionResult> Delete(string id)
        {
            var empToDelete = await _userManager.FindByIdAsync(id);
            if (empToDelete == null)
            {
                return NotFound();
            }
            var result = await _userManager.DeleteAsync(empToDelete);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest("Not succeeded!");
        }

    }        

}
