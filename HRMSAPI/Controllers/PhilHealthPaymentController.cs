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
    public class PhilHealthPaymentController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        IPhilHealthPaymentDBRepository _repo;
        IDataProtectionProvider _dataProtectionProvider;

        public PhilHealthPaymentController(UserManager<ApplicationUser> userManager, IPhilHealthPaymentDBRepository repo, IDataProtectionProvider dataProtectionProvider)
        {
            _userManager = userManager;
            _repo = repo;
            _dataProtectionProvider = dataProtectionProvider;
        }

        //Get All List of Payments
        //[Authorize(Roles = "Administrator, Manager, Employee")]
        [HttpGet]
        public IActionResult GetAll()
        {
            // Remove the Comment to Decrypt
            var paymentList = _repo.ListOfPhilHealthPayment();
            // var protector = _dataProtectionProvider.CreateProtector("PhilHealthNumber");
            // foreach (var payment in paymentList)
            // {
            //     payment.PhilHealthNumber = protector.Unprotect(payment.PhilHealthNumber);
            // }
            return Ok(paymentList);
        }

        //Get Payment By Id
        //[Authorize(Roles = "Administrator, Manager, Employee")]
        [HttpGet("{no}")]
        public IActionResult GetById([FromRoute]int no)
        {
            // Remove the Comment to Decrypt
            var paymentId = _repo.GetPhilHealthPaymentById(no);
            // var protector = _dataProtectionProvider.CreateProtector("PhilHealthNumber");
            // paymentId.PhilHealthNumber = protector.Unprotect(paymentId.PhilHealthNumber);
            return Ok(paymentId);
        }

        //Add Payment
        //[Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult Add([FromBody] AddPhilHealthPaymentDTO addDTO)
        {
            // Decrypt the Data
            var users = _userManager.Users.ToList();
            var protectId = _dataProtectionProvider.CreateProtector("SSSNumber", "PagIbigId", "PhilHealthId");

            foreach (var user in users)
            {
                user.SSSNumber = protectId.Unprotect(user.SSSNumber);
                user.PagIbigId = protectId.Unprotect(user.PagIbigId);
                user.PhilHealthId = protectId.Unprotect(user.PhilHealthId);
            }

            // Add and Encrypt
            var employee = users.FirstOrDefault(e => e.PhilHealthId == addDTO.PhilHealthNumber);
            var protector = _dataProtectionProvider.CreateProtector("PhilHealthNumber");
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
                        PhilHealthNumber = protector.Protect(addDTO.PhilHealthNumber),
                        FullName = employee.FirstName + " " + employee.MiddleName + " " + employee.LastName,
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
        //[Authorize(Roles = "Administrator")]
        [HttpPut("{no}")]
        public async Task<IActionResult> UpdatePaymentAsync([FromBody] EditPhilHealthPaymentDTO editPhilHealthPaymentDTO, [FromRoute] int no)
        {
            var payment = _repo.GetPhilHealthPaymentById(no);
            if (payment == null)
            {
                return NotFound();
            }
            // Decrypt the Data
            var users = _userManager.Users.ToList();
            var protectId = _dataProtectionProvider.CreateProtector("SSSNumber", "PagIbigId", "PhilHealthId");
            var protectId2 = _dataProtectionProvider.CreateProtector("PhilHealthNumber");

            foreach (var user in users)
            {
                user.SSSNumber = protectId.Unprotect(user.SSSNumber);
                user.PagIbigId = protectId.Unprotect(user.PagIbigId);
                user.PhilHealthId = protectId.Unprotect(user.PhilHealthId);
            }
            payment.PhilHealthNumber = protectId2.Unprotect(payment.PhilHealthNumber);

            //Encrypt and Update
            var employee = users.FirstOrDefault(e => e.PhilHealthId == payment.PhilHealthNumber);
            var protector2 = _dataProtectionProvider.CreateProtector("PhilHealthNumber");
            if (employee == null)
            {
                return BadRequest("Not Existing PagIbig Number");
            }

            if (editPhilHealthPaymentDTO != null)
            {
                if (ModelState.IsValid)
                {
                    payment.No = no;
                    payment.PhilHealthNumber = protector2.Protect(employee.PhilHealthId);
                    payment.FullName = employee.FirstName + " " + employee.MiddleName + " " + employee.LastName;
                    payment.Payment = editPhilHealthPaymentDTO.Payment;
                    payment.Month = editPhilHealthPaymentDTO.Month;
                    payment.Year = editPhilHealthPaymentDTO.Year;

                    _repo.UpdatePhilHealthPayment(payment, no);
                    return Ok(payment);
                }
                return BadRequest(ModelState);
            }
            return BadRequest("No resource found!");
        }

        //Delete Payment
        //[Authorize(Roles = "Administrator")]
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
