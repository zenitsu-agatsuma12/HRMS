using HRMSAPI.DTO;
using HRMSAPI.Models;
using HRMSAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HRMSAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PhilHealthController : ControllerBase
    {
        IPhilHealthRepository _repo;

        private UserManager<ApplicationUser> _userManager;

        public PhilHealthController(IPhilHealthRepository repo, UserManager<ApplicationUser> userManager)
        {
            _repo = repo;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_repo.ListOfPhilHealth());
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute]string id)
        {
            var getId = _repo.GetPhilHealthById(id);
            if (getId != null)
            {
                return NotFound();
            }
            return Ok(getId);
        }

        [HttpPost]
        public IActionResult Add([FromBody] AddPhilHealthDTO addPhilHealthDTO)
        {

            if (addPhilHealthDTO == null)
            {
                return BadRequest("No Resources found");
            }
            if (ModelState.IsValid)
            {
                var newdata = new PhilHealth()
                {
                    PhilHealthId = addPhilHealthDTO.PhilHealthId,
                    EmpId = addPhilHealthDTO.EmpId,
                    StartDate = addPhilHealthDTO.StartDate,
                    Status = addPhilHealthDTO.Status
                };
                var result = _repo.AddPhilHealth(newdata);
                return CreatedAtAction("GetById", new { id = result.EmpId }, result);
            }
            return BadRequest("No Resource Found");
        }
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute]string id)
        {
            var delete = _repo.GetPhilHealthById(id);
            if (delete == null)
            {
                return NotFound();
            }
            return Ok(_repo.DeletePhilHealth(id));
        }
    }
}
