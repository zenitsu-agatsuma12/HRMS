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
            ViewBag.PositionId = _repo.GetPositionList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(DepartmentPositioncs newDepartmentPositioncs)
        {
            if (ModelState.IsValid)
            {
                var newdesignation = _repo.AddDepartmentPositioncs(newDepartmentPositioncs);
                if(newdesignation == null)
                {
                    TempData["DesignationAlert"] = "This designation already exists.";
                    return RedirectToAction("Create");
                }
                TempData["DesignationAlert"] = "The designation is successfully added.";
                return RedirectToAction("List");
            }
            TempData["DesignationAlert"] = "Data is not valid to create the DepartmentPosition";
            return View();
        }
        
        //Update Designation
        [HttpGet]
        public IActionResult Update(int No)
        {
            ViewBag.DepartmentId = _repo.GetDepartmentList();
            ViewBag.PositionId = _repo.GetPositionList();
            DepartmentPositioncs departmentPositioncs = _repo.GetDepartmentPositionById(No);
            return View(departmentPositioncs);
        }
        [HttpPost]
        public  IActionResult Update(int No, DepartmentPositioncs departmentPositioncs)
        {
            ViewBag.DepartmentId = _repo.GetDepartmentList();
            ViewBag.PositionId = _repo.GetPositionList();
            
            var newdesignation = _repo.UpdateDepartmentPosition(No, departmentPositioncs);
            if (newdesignation == null)
            {
                TempData["DesignationAlert"] = "This Designation already exists.";
                return RedirectToAction("List");
            }
            TempData["DesignationAlert"] = "The Designation is Successfully Updated!";
            return RedirectToAction("List");
        }

        //Delete Desigantion
        public IActionResult Delete(int No)
        {
            var department = _repo.GetDepartmentPositionById(No);
            try
            {
                TempData["DesignationAlert"] = "The Designation is Successfully Deleted!";
                _repo.DeleteDepartmentPosition(No);
                return RedirectToAction("List");
            }
            catch
            {
                TempData["DesignationAlert"] = "It is not possible to delete this designation while there is an Employee assigned to it.";
                return RedirectToAction("List");
            }
        }
    }
}
