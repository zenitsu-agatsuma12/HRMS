using HRMS.Data;
using HRMS.Models;
using HRMS.Repository;
using HRMS.Repository.SqlRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Controllers
{
 //   [Authorize(Roles = "Human Resource")]
    public class PhilHealthController : Controller
    {
        IPhilHealthRepository _repo;
        HRMSDBContext _dbcontext;
        public PhilHealthController(IPhilHealthRepository repo)
        {
            this._repo = repo;
        }

        public IActionResult List(string searchValue)
        {
            var list = _repo.ListOfPhilHealth(searchValue);
            return View(list);
        }
        [HttpGet]
        public IActionResult Create(int empId, string startdate)
        {
            // ViewBag.EmpId = _repo.GetEmployeeList();
            var confirmID = _repo.ConfirmID(empId, startdate);
            if (confirmID == null)
            {
                PhilHealth PhilHealth = new PhilHealth();
                PhilHealth.EmpId = empId;
                PhilHealth.StartDate = startdate;

                return View(PhilHealth);
            }
            var getID = _repo.GetID(empId);

            return RedirectToAction("Update", "PhilHealth", new { PhilHealthId = getID });
        }
        [HttpPost]
        public IActionResult Create(PhilHealth newEmp)
        {
            if (ModelState.IsValid)
            {
                var emp = _repo.AddPhilHealth(newEmp);
                return RedirectToAction("List");
            }
            ViewData["Message"] = "Data is not valid to create the PhilHealth";
            return View();
        }

        [HttpGet]
        public IActionResult Update(string PhilHealthId)
        {
            PhilHealth PhilHealth = _repo.GetPhilHealthById(PhilHealthId); ;
            ViewBag.EmpId = _repo.GetEmployeeList();
            return View(PhilHealth);
        }
        [HttpPost]
        public IActionResult Update(string PhilHealthId, PhilHealth PhilHealth)
        {
            _repo.UpdatePhilHealth(PhilHealthId, PhilHealth);
            return RedirectToAction("List");
        }

        public IActionResult Details(string PhilHealthId)
        {
            var emp = _repo.GetPhilHealthById(PhilHealthId);
            return View(emp);
        }
        public IActionResult Delete(string PhilHealthId)
        {
            _repo.DeletePhilHealth(PhilHealthId);
            return RedirectToAction("List");
        }
    }
}
