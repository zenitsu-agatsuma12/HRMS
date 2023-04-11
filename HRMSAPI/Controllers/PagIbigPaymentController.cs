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
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_repo.ListOfPagIbigPayment());
        }

        //Get Payment By Id
        [HttpGet("{no}")]
        public IActionResult GetById([FromRoute] int no)
        {
            var paymentId = _repo.GetPagIbigPaymentById(no);
            return Ok(paymentId);
        }

        //Add Payment
        [HttpPost]
        public IActionResult Add([FromBody] AddPagIbigPaymentDTO addDTO)
        {
            var employee = _userManager.Users.FirstOrDefault(e => e.PagIbigId == addDTO.PagIbigNumber);
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
                        FullName = employee.FullName,
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
       /* [HttpPut]
        public IActionResult UpdatePayment([FromBody]EditPagIbigPaymentDTO editPagIbigPaymentDTO, [FromRoute]int no)
        {
            if (editPagIbigPaymentDTO == null)
                return BadRequest("No resource found");
            if (ModelState.IsValid)
            {
                var editPayment = new PagIbigPayment()
                {
                    No = editPagIbigPaymentDTO.No,
                    Payment = editPagIbigPaymentDTO.Payment,
                    Month = editPagIbigPaymentDTO.Month,
                    Year = editPagIbigPaymentDTO.Year
                };
                var result = _repo.UpdatePagIbigPayment(editPayment, no);
                return Ok(result);
            }
            return BadRequest(ModelState);
        } */


        //Delete Payment
        [HttpDelete]
        public IActionResult DeletePayment(int id)
        {
            var payment = _repo.GetPagIbigPaymentById(id);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }
    }
}
