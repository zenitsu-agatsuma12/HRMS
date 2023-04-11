using HRMS.Data;
using HRMS.Models;
using HRMS.Repository;
using HRMS.Repository.SqlRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Controllers
{
  //  [Authorize(Roles = "Human Resource")]
    public class DepartmentController : Controller
    {
        IDepartmentRepository _repo;
       
        public DepartmentController(IDepartmentRepository repo)
        {
            _repo=repo;
        }

        public IActionResult List(string searchValue)
        {
           var filter = _repo.Filter(searchValue);
            return View(filter);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Department newDept)
        {
            if (ModelState.IsValid)
            {
                var Dept = _repo.AddDepartment(newDept);
                return RedirectToAction("List");
            }
            ViewData["Message"] = "Data is not valid to create the Department";
            return View();
        }

        [HttpGet]
        public IActionResult Update(int DeptId)
        {
            Department Department = _repo.GetDepartmentById(DeptId); ;
            //ViewBag.DepartmentId = _repo.GetDepartmentList(Deptid);
            return View(Department);
        }
        [HttpPost]
        public IActionResult Update(int DeptId, Department Department)
        {
            _repo.UpdateDepartment(DeptId, Department);
            return RedirectToAction("List");
        }

        public IActionResult Details(int DeptId)
        {
            var Dept = _repo.GetDepartmentById(DeptId);
            return View(Dept);
        }
        public IActionResult Delete(int DeptId)
        {
            _repo.DeleteDepartment(DeptId);
            return RedirectToAction("List");
        }
    }
}
