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
    public class PositionController : Controller
    {
        IPositionRepository _repo;

        public PositionController(IPositionRepository repo)
        {
            _repo = repo;
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
        public IActionResult Create(Position newDept)
        {
            if (ModelState.IsValid)
            {
                var pos = _repo.AddPosition(newDept);
                TempData["PositionAlert"] = pos.Name + " Successfully Added!";
                return RedirectToAction("List");
            }
            ViewData["PositionAlert"] = "Data is not valid to create the Position";
            return View();
        }

        [HttpGet]
        public IActionResult Update(int PosId)
        {
            Position Position = _repo.GetPositionById(PosId); ;
            return View(Position);
        }
        [HttpPost]
        public IActionResult Update(int PosId, Position Position)
        {
            TempData["PositionAlert"] ="Update Successfully!";
            _repo.UpdatePosition(PosId, Position);
            return RedirectToAction("List");
        }

        public IActionResult Details(int PosId)
        {
            var Dept = _repo.GetPositionById(PosId);
            return View(Dept);
        }
        public IActionResult Delete(int PosId)
        {
            var position = _repo.GetPositionById(PosId);
            try
            {
                _repo.DeletePosition(PosId);
                TempData["PositionAlert"] = position.Name + " is Successfully Deleted!";
                return RedirectToAction("List");
            }
            catch
            {
                TempData["PositionAlert"] = "It is not possible to delete this position while there is an employee assigned to it.";
                return RedirectToAction("List");
            }
           
        }
    }
}
