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
                TempData["DepartmentAlert"] = Dept.DeptName + " Deapartment Successfully Added!";
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
            TempData["DepartmentAlert"] = " Update Successfully!";
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
            var department = _repo.GetDepartmentById(DeptId);
            try
            {
                TempData["DepartmentAlert"] = department.DeptName + " is Successfully Deleted!";
                _repo.DeleteDepartment(DeptId);
                return RedirectToAction("List");
            }
            catch
            {
                
                TempData["DepartmentAlert"] = "It is not possible to delete this department while there is an employee assigned to it.";
                return RedirectToAction("List");
            }
           
        }
    }
}
