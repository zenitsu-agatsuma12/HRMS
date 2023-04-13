using HRMS.Data;
using HRMS.Models;
using HRMS.Repository;
using HRMS.Repository.SqlRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class DepartmentPositionController : Controller
    {

        IDepartmentPositionRepository _repo ;
       // HRMSDBContext _dbcontext;
        public DepartmentPositionController(IDepartmentPositionRepository repo)
        {
            _repo = repo;
        }
        public IActionResult List(string searchOption, string searchValue)
        {
            ViewBag.Departments = _repo.GetDepartmentList();
            var filter = _repo.GetFilter(searchOption, searchValue);
            return View(filter);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.DepartmentId = _repo.GetDepartmentList();
            ViewBag.PositionId = _repo.GetPosition();
            return View();
        }
        [HttpPost]
        public IActionResult Create(DepartmentPositioncs newDepartmentPositioncs)
        {
            if (ModelState.IsValid)
            {
                _repo.AddDepartmentPositioncs(newDepartmentPositioncs);
                return RedirectToAction("List");
            }
            ViewData["Message"] = "Data is not valid to create the DepartmentPosition";
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
