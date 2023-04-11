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
    public class PhilHealthPaymentController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        IPhilHealthPaymentDBRepository _repo;

        public PhilHealthPaymentController(UserManager<ApplicationUser> userManager, IPhilHealthPaymentDBRepository repo)
        {
            _userManager = userManager;
            _repo = repo;
        }

        //Get All List of Payments
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_repo.ListOfPhilHealthPayment());
        }

        //Get Payment By Id
        [HttpGet("{no}")]
        public IActionResult GetById([FromRoute]int no)
        {
            var paymentId = _repo.GetPhilHealthPaymentById(no);
            return Ok(paymentId);
        }

        //Add Payment
        [HttpPost]
        public IActionResult Add([FromBody] AddPhilHealthPaymentDTO addDTO)
        {
            var employee = _userManager.Users.FirstOrDefault(e => e.PhilHealthId == addDTO.PhilHealthNumber);
            if (employee == null)
            {
                return BadRequest("Not Existing PhilHealth Number");
            }
            if (addDTO != null)
            {
                if (ModelState.IsValid)
                {
                    var addPayment = new PhilHealthPayment()
                    {
                        PhilHealthNumber = addDTO.PhilHealthNumber,
                        FullName = employee.FullName,
                        Payment = addDTO.Payment,
                        Month = addDTO.Month,
                        Year = addDTO.Year
                    };
                    var result = _repo.AddPhilHealthPayment(addPayment);
                    return Ok(result);
                }
                return BadRequest(ModelState);
            }
            return BadRequest("No resource found!");
        }

        //Update Payment


        //Delete Payment
        [HttpDelete]
        public IActionResult DeletePayment(int no)
        {
            var payment = _repo.GetPhilHealthPaymentById(no);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }
    }
}
