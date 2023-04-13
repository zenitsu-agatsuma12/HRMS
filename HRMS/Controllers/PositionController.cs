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
                var Dept = _repo.AddPosition(newDept);
                return RedirectToAction("List");
            }
            ViewData["Message"] = "Data is not valid to create the Position";
            return View();
        }

        [HttpGet]
        public IActionResult Update(int PosId)
        {
            Position Position = _repo.GetPositionById(PosId); ;
            //ViewBag.PositionId = _repo.GetPositionList(Deptid);
            return View(Position);
        }
        [HttpPost]
        public IActionResult Update(int PosId, Position Position)
        {
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
            _repo.DeletePosition(PosId);
            return RedirectToAction("List");
        }
    }
}
