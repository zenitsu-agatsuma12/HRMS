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
    public class SSSPaymentController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        ISSSPaymentRepository _repo;

        public SSSPaymentController(UserManager<ApplicationUser> userManager, ISSSPaymentRepository repo)
        {
            _userManager = userManager;
            _repo = repo;
        }

        //Get All List of Payments
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_repo.ListOfSSSPayment());
        }

        //Get Payment By Id
        [HttpGet("{no}")]
        public IActionResult GetById([FromRoute] int no)
        {
            var paymentId = _repo.GetSSSPaymentById(no);
            return Ok(paymentId);
        }

        //Add Payment
        [HttpPost]
        public IActionResult Add([FromBody] AddSSSPaymentDTO addDTO)
        {
            var employee = _userManager.Users.FirstOrDefault(e => e.SSSNumber == addDTO.SSSNumber);
            if (addDTO != null)
            {
                if (ModelState.IsValid)
                {
                    var addPayment = new SSSPayment()
                    {
                        SSSNumber = addDTO.SSSNumber,
                        FullName = employee.FullName,
                        Payment = addDTO.Payment,
                        Month = addDTO.Month,
                        Year = addDTO.Year
                    };
                    var result = _repo.AddSSSPayment(addPayment);
                    return Ok(result);
                }
                return BadRequest(ModelState);
            }
            return BadRequest("No resource found!");
        }

        //Update Payment


        //Delete Payment
        [HttpDelete]
        public IActionResult DeletePayment(int id)
        {
            var payment = _repo.GetSSSPaymentById(id);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }
    }
}
