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
    public class PagIbigPaymentController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        IPagIbigPaymentRepository _repo;

        public PagIbigPaymentController(UserManager<ApplicationUser> userManager, IPagIbigPaymentRepository repo)
        {
            _userManager = userManager;
            _repo = repo;
        }

        //Get All List of Payments
        [Authorize(Roles = "Administrator, Manager, Employee")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var paymentList = _repo.ListOfPagIbigPayment();
            return Ok(paymentList);
        }

        //Get Payment By Id
        [Authorize(Roles = "Administrator, Manager, Employee")]
        [HttpGet("{no}")]
        public IActionResult GetById([FromRoute] int no)
        {
            var paymentId = _repo.GetPagIbigPaymentById(no);
            return Ok(paymentId);
        }

        //Add Payment
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult Add([FromBody] AddPagIbigPaymentDTO addDTO)
        {
            var users = _userManager.Users.ToList();
            var employee = users.FirstOrDefault(e => e.PagIbigId == addDTO.PagIbigNumber);
            if (employee == null)
            {
                return BadRequest("Not Existing PagIbig Number");
            }
            if (addDTO != null)
            {
                if (ModelState.IsValid)
                {
                    var addPayment = new PagIbigPayment()
                    {
                        PagIbigNumber = addDTO.PagIbigNumber,
                        FullName = employee.FirstName + " " + employee.MiddleName + " " + employee.LastName,
                        Payment = addDTO.Payment,
                        Month = addDTO.Month,
                        Year = addDTO.Year
                    };
                    var result = _repo.AddPagIbigPayment(addPayment);
                    return Ok(result);
                }
                return BadRequest(ModelState);
            }
            return BadRequest("No resource found!");
        }

        //Update Payment
        [Authorize(Roles = "Administrator")]
        [HttpPut("{no}")]
        public async Task<IActionResult> UpdatePaymentAsync([FromBody]EditPagIbigPaymentDTO editPagIbigPaymentDTO, [FromRoute]int no)
        {
            var payment = _repo.GetPagIbigPaymentById(no);
            if (payment == null)
            {  
                return NotFound();
            }
            var users = _userManager.Users.ToList();
            var employee = users.FirstOrDefault(e => e.PagIbigId == payment.PagIbigNumber);
            if (employee == null)
            {
                return BadRequest("Not Existing PagIbig Number");
            }

            if (editPagIbigPaymentDTO != null)
            {
                if (ModelState.IsValid)
                {
                    payment.No = no;
                    payment.PagIbigNumber = employee.PagIbigId;
                    payment.FullName = employee.FirstName + " " + employee.MiddleName + " " + employee.LastName;
                    payment.Payment = editPagIbigPaymentDTO.Payment;
                    payment.Month = editPagIbigPaymentDTO.Month;
                    payment.Year = editPagIbigPaymentDTO.Year;
                    
                    _repo.UpdatePagIbigPayment(payment , no);
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
            var payment = _repo.GetPagIbigPaymentById(no);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(_repo.DeletePagIbigPayment(no));
        }
    }
}
