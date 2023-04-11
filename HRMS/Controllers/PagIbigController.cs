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
    public class PagIbigController : Controller
    {
        IPagIbigRepository _repo;
        HRMSDBContext _dbcontext;
        public PagIbigController(IPagIbigRepository repo)
        {
            this._repo = repo;
        }

        public IActionResult List(string searchValue)
        {
            var list = _repo.ListOfPagIbig(searchValue);
            return View(list);
        }
        [HttpGet]
        public IActionResult Create(int empId, string startdate)
        {
            // ViewBag.EmpId = _repo.GetEmployeeList();
            var confirmID = _repo.ConfirmID(empId, startdate);
            if (confirmID == null)
            {
                PagIbig PagIbig = new PagIbig();
                PagIbig.EmpId = empId;
                PagIbig.StartDate = startdate;

                return View(PagIbig);
            }
            var getID = _repo.GetID(empId);

            return RedirectToAction("Update", "PagIbig", new { PagIbigId = getID });
        }
        [HttpPost]
        public IActionResult Create(PagIbig newEmp)
        {
            if (ModelState.IsValid)
            {
                var emp = _repo.AddPagIbig(newEmp);
                return RedirectToAction("List");
            }
            ViewData["Message"] = "Data is not valid to create the PagIbig";
            return View();
        }

        [HttpGet]
        public IActionResult Update(string PagIbigId)
        {
            PagIbig PagIbig = _repo.GetPagIbigById(PagIbigId); ;
            ViewBag.EmpId = _repo.GetEmployeeList();
            return View(PagIbig);
        }
        [HttpPost]
        public IActionResult Update(string PagIbigId, PagIbig PagIbig)
        {
            _repo.UpdatePagIbig(PagIbigId, PagIbig);
            return RedirectToAction("List");
        }

        public IActionResult Details(string PagIbigId)
        {
            var emp = _repo.GetPagIbigById(PagIbigId);
            return View(emp);
        }
        public IActionResult Delete(string PagIbigId)
        {
            _repo.DeletePagIbig(PagIbigId);
            return RedirectToAction("List");
        }
    }
}
