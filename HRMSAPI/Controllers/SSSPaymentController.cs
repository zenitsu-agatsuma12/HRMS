using HRMSAPI.DTO;
using HRMSAPI.Models;
using HRMSAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HRMSAPI.Controllers
{
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
        [Authorize(Roles = "Administrator, Manager, Employee")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var paymentList = _repo.ListOfSSSPayment();
            return Ok(paymentList);
        }

        //Get Payment By Id
        [Authorize(Roles = "Administrator, Manager, Employee")]
        [HttpGet("{no}")]
        public IActionResult GetById([FromRoute] int no)
        {
            var paymentId = _repo.GetSSSPaymentById(no);
            return Ok(paymentId);
        }

        //Add Payment
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult Add([FromBody] AddSSSPaymentDTO addDTO)
        {
            var users = _userManager.Users.ToList();
            var employee = users.FirstOrDefault(e => e.SSSNumber == addDTO.SSSNumber);
            if (employee == null)
            {
                return BadRequest("Not Existing SSS Number");
            }
            if (addDTO != null)
            {
                if (ModelState.IsValid)
                {
                    var addPayment = new SSSPayment()
                    {
                        SSSNumber = addDTO.SSSNumber,
                        FullName = employee.FirstName + " " + employee.MiddleName + " " + employee.LastName,
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
        [Authorize(Roles = "Administrator")]
        [HttpPut("{no}")]
        public async Task<IActionResult> UpdatePaymentAsync([FromBody] EditSSSPaymentDTO editSSSPaymentDTO, [FromRoute] int no)
        {
            var payment = _repo.GetSSSPaymentById(no);
            if (payment == null)
            {
                return NotFound();
            }

            var users = _userManager.Users.ToList();

            var employee = users.FirstOrDefault(e => e.SSSNumber == payment.SSSNumber);
            if (employee == null)
            {
                return BadRequest("Not Existing PagIbig Number");
            }

            if (editSSSPaymentDTO != null)
            {
                if (ModelState.IsValid)
                {
                    payment.No = no;
                    payment.SSSNumber = employee.SSSNumber;
                    payment.FullName = employee.FirstName + " " + employee.MiddleName + " " + employee.LastName;
                    payment.Payment = editSSSPaymentDTO.Payment;
                    payment.Month = editSSSPaymentDTO.Month;
                    payment.Year = editSSSPaymentDTO.Year;

                    _repo.UpdateSSSPayment(payment, no);
                    return Ok(payment);
                }
                return BadRequest(ModelState);
            }
            return BadRequest("No resource found!");
        }

        //Delete Payment
        [Authorize(Roles = "Administrator")]
        [HttpDelete]
        public IActionResult DeletePayment(int no)
        {
            var payment = _repo.GetSSSPaymentById(no);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(_repo.DeleteSSSPayment(no));
        }
    }
}
