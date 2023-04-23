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
        [Authorize(Roles = "Administrator, Manager, Employee")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var paymentList = _repo.ListOfPhilHealthPayment();
            return Ok(paymentList);
        }

        //Get Payment By Id
        [Authorize(Roles = "Administrator, Manager, Employee")]
        [HttpGet("{no}")]
        public IActionResult GetById([FromRoute]int no)
        {
            var paymentId = _repo.GetPhilHealthPaymentById(no);
            if (paymentId == null)
            {
                return BadRequest("No Resource Found!");
            }
                return Ok(paymentId);
        }

        //Add Payment
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult Add([FromBody] AddPhilHealthPaymentDTO addDTO)
        {
            var users = _userManager.Users.ToList();
            var employee = users.FirstOrDefault(e => e.PhilHealthId == addDTO.PhilHealthNumber);

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
                        FullName = employee.FirstName + " " + employee.MiddleName + " " + employee.LastName,
                        Payment = addDTO.Payment,
                        Month = addDTO.Month,
                        Year = addDTO.Year,
                        status = true
                    };
                    var result = _repo.AddPhilHealthPayment(addPayment);
                    return Ok(result);
                }
                return BadRequest(ModelState);
            }
            return BadRequest("No resource found!");
        }

        //Update Payment
        [Authorize(Roles = "Administrator")]
        [HttpPut("{no}")]
        public async Task<IActionResult> UpdatePaymentAsync([FromBody] EditPhilHealthPaymentDTO editPhilHealthPaymentDTO, [FromRoute] int no)
        {
            var payment = _repo.GetPhilHealthPaymentById(no);
            if (payment == null)
            {
                return NotFound();
            }

            var users = _userManager.Users.ToList();

            var employee = users.FirstOrDefault(e => e.PhilHealthId == payment.PhilHealthNumber);
            if (employee == null)
            {
                return BadRequest("Not Existing PagIbig Number");
            }

            if (editPhilHealthPaymentDTO != null)
            {
                if (ModelState.IsValid)
                {
                    payment.No = no;
                    payment.PhilHealthNumber = employee.PhilHealthId;
                    payment.FullName = employee.FirstName + " " + employee.MiddleName + " " + employee.LastName;
                    payment.Payment = editPhilHealthPaymentDTO.Payment;
                    payment.Month = editPhilHealthPaymentDTO.Month;
                    payment.Year = editPhilHealthPaymentDTO.Year;
                    payment.status = true;

                    _repo.UpdatePhilHealthPayment(payment, no);
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
            var payment = _repo.GetPhilHealthPaymentById(no);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(_repo.DeletePhilHealthPayment(no));
        }
    }
}
