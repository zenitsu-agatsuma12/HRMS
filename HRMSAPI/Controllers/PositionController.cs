using HRMSAPI.DTO;
using HRMSAPI.Models;
using HRMSAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace HRMSAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        IPositionRepository _repo;

        public PositionController(IPositionRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]

        public IActionResult GetAllPosition()
        {
            return Ok(_repo.ListOfPosition());
        }

        [HttpGet("{id}")]

        public IActionResult GetById([FromRoute]int id)
        {
            var pos = _repo.GetPositionById(id);
            if (pos == null)
                return BadRequest("No Resource Found!");
            return Ok(pos);
        }

        [HttpPost]

        public IActionResult AddPosition(AddPositionDTO position)
        {
            if (position == null)
                return BadRequest("No Resource Found!");
            if (ModelState.IsValid)
            {
                var pos = new Position
                {
                    Name = position.Name,
                };
                return Ok(_repo.AddPosition(pos));
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]

        public IActionResult UpdatePosition([FromBody]Position position, [FromRoute] int id)
        {
            if (position == null)
                return BadRequest("No Resource Found!");
            if (ModelState.IsValid)
            {
                Ok(_repo.UpdatePosition(id, position));
            }
            return Ok(position);
        }

        [HttpDelete]

        public IActionResult DeletePosition(int id)
        {
            var pos = _repo.GetPositionById(id);
            if (pos == null)
                return BadRequest("No Resource Found!");
            return Ok(_repo.DeletePosition(id));
        }
    }
}
