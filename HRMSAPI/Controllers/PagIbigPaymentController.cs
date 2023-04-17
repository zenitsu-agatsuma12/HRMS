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
        IDataProtectionProvider _dataProtectionProvider;

        public PagIbigPaymentController(UserManager<ApplicationUser> userManager, IPagIbigPaymentRepository repo, IDataProtectionProvider dataProtectionProvider)
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
            //Remove Comment if you want to decrypt
            var paymentList = _repo.ListOfPagIbigPayment();
            // var protector = _dataProtectionProvider.CreateProtector("PagIbigNumber");
            // foreach (var payment in paymentList)
            // {
            //     payment.PagIbigNumber = protector.Unprotect(payment.PagIbigNumber);
            // }
            return Ok(paymentList);
        }

        //Get Payment By Id
        [Authorize(Roles = "Administrator, Manager, Employee")]
        [HttpGet("{no}")]
        public IActionResult GetById([FromRoute] int no)
        {
            //Remove Comment if you want to decrypt
            var paymentId = _repo.GetPagIbigPaymentById(no);
            //var protector = _dataProtectionProvider.CreateProtector("PagIbigNumber");
            //paymentId.PagIbigNumber = protector.Unprotect(paymentId.PagIbigNumber);
            return Ok(paymentId);
        }

        //Add Payment
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult Add([FromBody] AddPagIbigPaymentDTO addDTO)
        {
            // Decrypt Data
            var users = _userManager.Users.ToList();
            var protectId = _dataProtectionProvider.CreateProtector("SSSNumber", "PagIbigId", "PhilHealthId");

            foreach (var user in users)
            {
                user.SSSNumber = protectId.Unprotect(user.SSSNumber);
                user.PagIbigId = protectId.Unprotect(user.PagIbigId);
                user.PhilHealthId = protectId.Unprotect(user.PhilHealthId);
            }

            // Add and Encrypt
            var employee = users.FirstOrDefault(e => e.PagIbigId == addDTO.PagIbigNumber);
            var protector = _dataProtectionProvider.CreateProtector("PagIbigNumber");
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
                        PagIbigNumber = protector.Protect(addDTO.PagIbigNumber),
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
        //[Authorize(Roles = "Administrator")]
        [HttpPut("{no}")]
        public async Task<IActionResult> UpdatePaymentAsync([FromBody]EditPagIbigPaymentDTO editPagIbigPaymentDTO, [FromRoute]int no)
        {
            var payment = _repo.GetPagIbigPaymentById(no);
            if (payment == null)
            {  
                return NotFound();
            }
            // Decrypt the Data
            var users = _userManager.Users.ToList();
            var protectId = _dataProtectionProvider.CreateProtector("SSSNumber", "PagIbigId", "PhilHealthId");
            var protectId2 = _dataProtectionProvider.CreateProtector("PagIbigNumber");

            foreach (var user in users)
            {
                user.SSSNumber = protectId.Unprotect(user.SSSNumber);
                user.PagIbigId = protectId.Unprotect(user.PagIbigId);
                user.PhilHealthId = protectId.Unprotect(user.PhilHealthId);  
            }
            payment.PagIbigNumber = protectId2.Unprotect(payment.PagIbigNumber);

            // Encrypt and Update
            var employee = users.FirstOrDefault(e => e.PagIbigId == payment.PagIbigNumber);
            var protector2 = _dataProtectionProvider.CreateProtector("PagIbigNumber");
            if (employee == null)
            {
                return BadRequest("Not Existing PagIbig Number");
            }

            if (editPagIbigPaymentDTO != null)
            {
                if (ModelState.IsValid)
                {
                    payment.No = no;
                    payment.PagIbigNumber = protector2.Protect(employee.PagIbigId);
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
