using HRMSAPI.DTO;
using HRMSAPI.Models;
using HRMSAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
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
        IDataProtectionProvider _dataProtectionProvider;

        public SSSPaymentController(UserManager<ApplicationUser> userManager, ISSSPaymentRepository repo, IDataProtectionProvider dataProtectionProvider)
        {
            _userManager = userManager;
            _repo = repo;
            _dataProtectionProvider = dataProtectionProvider;
        }

        //Get All List of Payments
        [Authorize(Roles = "Administrator, Manager, Employee")]
        [HttpGet]
        public IActionResult GetAll()
        {
            //Remove comment to Decrypt Data
            var paymentList = _repo.ListOfSSSPayment();
            // var protector = _dataProtectionProvider.CreateProtector("SSSNumber");
            // foreach (var payment in paymentList)
            // {
            //     payment.SSSNumber = protector.Unprotect(payment.SSSNumber);
            // }
            return Ok(paymentList);
        }

        //Get Payment By Id
        [Authorize(Roles = "Administrator, Manager, Employee")]
        [HttpGet("{no}")]
        public IActionResult GetById([FromRoute] int no)
        {
            //Remove comment to Decrypt
            var paymentId = _repo.GetSSSPaymentById(no);
            //var protector = _dataProtectionProvider.CreateProtector("SSSNumber");
            //paymentId.SSSNumber = protector.Unprotect(paymentId.SSSNumber);
            return Ok(paymentId);
        }

        //Add Payment
        // [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult Add([FromBody] AddSSSPaymentDTO addDTO)
        {
            //Decrypt the Data
            var users = _userManager.Users.ToList();
            var protectId = _dataProtectionProvider.CreateProtector("SSSNumber", "PagIbigId", "PhilHealthId");

            foreach (var user in users)
            {
                user.SSSNumber = protectId.Unprotect(user.SSSNumber);
                user.PagIbigId = protectId.Unprotect(user.PagIbigId);
                user.PhilHealthId = protectId.Unprotect(user.PhilHealthId);
            }

            //Add and Encrypt
            var protector = _dataProtectionProvider.CreateProtector("SSSNumber");
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
                        SSSNumber = protector.Protect(addDTO.SSSNumber),
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
        // [Authorize(Roles = "Administrator")]
        [HttpPut("{no}")]
        public async Task<IActionResult> UpdatePaymentAsync([FromBody] EditSSSPaymentDTO editSSSPaymentDTO, [FromRoute] int no)
        {
            var payment = _repo.GetSSSPaymentById(no);
            if (payment == null)
            {
                return NotFound();
            }

            // Decrypt the Data
            var users = _userManager.Users.ToList();
            var protectId = _dataProtectionProvider.CreateProtector("SSSNumber", "PagIbigId", "PhilHealthId");
            var protectId2 = _dataProtectionProvider.CreateProtector("SSSNumber");

            foreach (var user in users)
            {
                user.SSSNumber = protectId.Unprotect(user.SSSNumber);
                user.PagIbigId = protectId.Unprotect(user.PagIbigId);
                user.PhilHealthId = protectId.Unprotect(user.PhilHealthId);
            }
            payment.SSSNumber = protectId2.Unprotect(payment.SSSNumber);

            //Update and Encrypt
            var employee = users.FirstOrDefault(e => e.SSSNumber == payment.SSSNumber);
            var protector = _dataProtectionProvider.CreateProtector("SSSNumber");
            if (employee == null)
            {
                return BadRequest("Not Existing PagIbig Number");
            }

            if (editSSSPaymentDTO != null)
            {
                if (ModelState.IsValid)
                {
                    payment.No = no;
                    payment.SSSNumber = protector.Protect(employee.SSSNumber);
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
