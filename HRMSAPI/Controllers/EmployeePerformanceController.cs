using HRMSAPI.DTO;
using HRMSAPI.Models;
using HRMSAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMSAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeePerformanceController : ControllerBase
    {
        IEmployeePerformanceDBRepository _repo;
        IDataProtectionProvider _dataProtectionProvider;
        private UserManager<ApplicationUser> _userManager;
        public EmployeePerformanceController(IEmployeePerformanceDBRepository repo , UserManager<ApplicationUser> userManager, IDataProtectionProvider dataProtectionProvider)
        {
            _repo = repo;
            _userManager = userManager;
            _dataProtectionProvider = dataProtectionProvider;
        }

        // List of Employee Performance
        [Authorize(Roles = "Administrator, Manager, Employee")]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_repo.ListOfEmployeePerformance(null));
        }

        //Get Performance By Id
        [Authorize(Roles = "Administrator, Manager, Employee")]
        [HttpGet("{id}")]
        public IActionResult GetId([FromRoute]int id)
        {
            var empperId = _repo.GetEmployeePerformanceById(id);
            if (empperId == null)
            {
                return NotFound();
            }
            return Ok(empperId);
        }

        // For Adding 
        [Authorize(Roles = "Administrator, Manager")]
        [HttpPost]
        public IActionResult Add([FromBody]AddEmployeePerformanceDTO addDTO)
        {
            var employee = _userManager.Users.FirstOrDefault(e => e.Id == addDTO.userID);
            if (addDTO != null)
            {
                if (ModelState.IsValid)
                {
                    var addEmpPer = new EmployeePerformance()
                    {
                        userID = addDTO.userID,
                        EmployeeName = employee.FirstName + " " + employee.MiddleName + " " + employee.LastName,
                        About = addDTO.About,
                        PerformanceReview = addDTO.PerformanceReview,
                        ReviewBy = "Admin",
                        Status = true,
                        DateReview = addDTO.DateReview      
                    };
                    var newData = _repo.AddEmployeePerformance(addEmpPer);
                    return Ok(newData);
                }
            }
            return BadRequest("No resource found!");
        }

        // For Updating
        [Authorize(Roles = "Administrator, Manager")]
        [HttpPut("{no}")]
        public async Task<IActionResult> UpdateAsync([FromBody]EditEmployeePerformanceDTO editDTO, [FromRoute] int no)
        {
            var empPerId = _repo.ListOfEmployeePerformance(null).FirstOrDefault(e => e.No == no);
            var empUser = await _userManager.Users.FirstOrDefaultAsync(e => e.Id == empPerId.userID);
            if (empPerId != null)
            {
                empPerId.No = no;
                empPerId.EmployeeName = empUser.FirstName+" "+ empUser.MiddleName+" "+ empUser.LastName;
                empPerId.About = editDTO.About;
                empPerId.PerformanceReview = editDTO.PerformanceReview;
                empPerId.DateReview = editDTO.DateReview;
                empPerId.userID = empUser.Id;
                empPerId.Status = false;
                empPerId.ReviewBy = "Admin";
            }
            var result =  _repo.UpdateEmployeePerformance(no, empPerId);
            if (result != null)
            {
                return Ok(result);
            }
                return BadRequest("No resource found");
            
        }

        // For Delete
        [Authorize(Roles = "Administrator, Manager")]
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var delete = _repo.DeleteEmployeePerformance(id);
            if (delete == null)
            {
                return NotFound();
            }
            return Ok(delete);
        }

    }
}
