using HRMS.Data;
using HRMS.Models;
using HRMS.Repository;
using HRMS.Repository.SqlRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HRMS.Controllers
{
 //   [Authorize(Roles = "Human Resource")]
    public class SSSController : Controller
    {
        ISSSRepository _repo;
        HRMSDBContext _dbcontext;
        public SSSController(ISSSRepository repo)
        {
            this._repo = repo;
        }

        public IActionResult List(string searchValue)
        {
            var list = _repo.ListOfSSS(searchValue);
            return View(list);
        }
        [HttpGet]
        public IActionResult Create(int empId, string startdate)
        {
            // ViewBag.EmpId = _repo.GetEmployeeList();
            var confirmID = _repo.ConfirmID(empId, startdate);
            if (confirmID == null)
            {
                SSS SSS = new SSS();
                SSS.EmpId = empId;
                SSS.StartDate = startdate;

                return View(SSS);
            }
            var getID = _repo.GetID(empId);

            return RedirectToAction("Update", "SSS", new { SSSId = getID });
        }
        [HttpPost]
        public IActionResult Create(SSS newEmp)
        {
            if (ModelState.IsValid)
            {
                var emp = _repo.AddSSS(newEmp);
                return RedirectToAction("List");
            }
            ViewData["Message"] = "Data is not valid to create the SSS";
            return View();
        }

        [HttpGet]
        public IActionResult Update(string SSSId)
        {
            SSS SSS = _repo.GetSSSById(SSSId); ;
            ViewBag.EmpId = _repo.GetEmployeeList();
            return View(SSS);
        }
        [HttpPost]
        public IActionResult Update(string SSSId, SSS SSS)
        {
            _repo.UpdateSSS(SSSId, SSS);
            return RedirectToAction("List");
        }

        public IActionResult Details(string SSSId)
        {
            var emp = _repo.GetSSSById(SSSId);
            return View(emp);
        }
        public IActionResult Delete(string SSSId)
        {
            _repo.DeleteSSS(SSSId);
            return RedirectToAction("List");
        }
    }
}
