﻿using HRMSAPI.DTO;
using HRMSAPI.Models;
using HRMSAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMSAPI.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        IDepartmentRepository _repo;

        public DepartmentController(IDepartmentRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]

        public IActionResult GetAllDepartment()
        {
            return Ok(_repo.ListOfDepartment());
        }

        [HttpGet("{deptId}")]
        public IActionResult GetById([FromRoute]int deptId)
        {
            var value = _repo.GetDepartmentById(deptId);
            if (value == null)
                return BadRequest("No resources found!");
            return Ok(value);
        }

        [HttpPost]
        public IActionResult Add([FromBody] AddDepartmentDTO departmentDTO)
        {
            if (departmentDTO == null)
                BadRequest("No resource found");
            if (ModelState.IsValid)
            {
                var dept = new Department()
                {
                    DeptName = departmentDTO.DeptName
                };
                var newdata = _repo.AddDepartment(dept);
                return Ok(newdata);
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]

        public IActionResult Update([FromBody]AddDepartmentDTO editDTO,[FromRoute] int id)
        {
            var department = _repo.GetDepartmentById(id);
            if (department == null)
            {
               return BadRequest("No resource found");
            }
            department.DeptId = id;
            department.DeptName = editDTO.DeptName;

            var result = _repo.UpdateDepartment(id, department);
            if (result != null)
            {
                return Ok(result);
            }
                return BadRequest("Data Failed to Update!");

        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var dept = _repo.GetDepartmentById(id);
            if (dept == null)
                return BadRequest("No resource found");
            return Ok(_repo.DeleteDepartment(id));
        }

    }
}
