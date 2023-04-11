using HRMS.Data;
using HRMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Repository.SqlRepository
{
    public class DepartmentPositionDBRepository: IDepartmentPositionRepository
    {
        HRMSDBContext _dbcontext;
        public DepartmentPositionDBRepository(HRMSDBContext dbContext)
        {
            _dbcontext = dbContext;
        }
   
        public DepartmentPositioncs AddDepartmentPositioncs(DepartmentPositioncs newDepartmentPosition)
        {
            _dbcontext.Add(newDepartmentPosition);
            _dbcontext.SaveChanges();
            return newDepartmentPosition;
        }

        public DepartmentPositioncs DeleteDepartmentPosition(int DepartmentPositionId)
        {
            var Emp = GetDepartmentPositionById(DepartmentPositionId);
            if (Emp != null)
            {
                _dbcontext.DepartmentPositions.UpdateRange(Emp);
                _dbcontext.SaveChanges();
            }
            return Emp;
        }

        public DepartmentPositioncs GetDepartmentPositionById(int Id)
        {
            return _dbcontext.DepartmentPositions.Include(d => d.Department).AsNoTracking().ToList().FirstOrDefault(x => x.No == Id);
        }

        public List<DepartmentPositioncs> ListOfDepartmentPosition()
        {
            return _dbcontext.DepartmentPositions.AsNoTracking().ToList();
        }

        public DepartmentPositioncs UpdateDepartmentPosition(int DepartmentPositionId, DepartmentPositioncs newDepartmentPosition)
        {
            _dbcontext.DepartmentPositions.Update(newDepartmentPosition);
            _dbcontext.SaveChanges();
            return newDepartmentPosition;
        }
        public List<SelectListItem> GetDepartmentList()
        {
            var listDept = new List<SelectListItem>();
            List<Department> departments = _dbcontext.Departments.ToList();
            listDept = departments.Select(dept => new SelectListItem
            {
                Value = (dept.DeptId).ToString(),
                Text = dept.DeptName
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "Select Department"
            };
            listDept.Insert(0, defItem);
            return listDept;
        }

        public List<SelectListItem> GetPosition()
        {
            var listPosition = new List<SelectListItem>();
            List<Position> departments = _dbcontext.Positions.ToList();
            listPosition = departments.Select(dept => new SelectListItem
            {
                Value = (dept.PosId).ToString(),
                Text = dept.Name
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "Select Position"
            };
            listPosition.Insert(0, defItem);
            return listPosition;
        }

        public List<DepartmentPositioncs> ListDepartmentPositioncs()
        {
            List<DepartmentPositioncs> departmentPositions = _dbcontext.DepartmentPositions
                                                                    .Include(d => d.Department)
                                                                    .Include(p => p.Position)
                                                                    .ToList();
            return departmentPositions;
        }

        public List<DepartmentPositioncs> GetFilter(string searchOption, string searchValue)
        {
            if (!string.IsNullOrEmpty(searchOption) && !string.IsNullOrEmpty(searchValue))
            {
                List<DepartmentPositioncs> departmentPositioncs = _dbcontext.DepartmentPositions
                    .Include(d => d.Department)
                    .Include(p => p.Position)
                    .Where(e => (e.DepartmentId.ToString().Contains(searchOption))
                             && e.Position.Name.Contains(searchValue))
                                 .ToList();
                return departmentPositioncs;
            }
            else if (!string.IsNullOrEmpty(searchOption) && string.IsNullOrEmpty(searchValue))
            {
                List<DepartmentPositioncs> departmentPositioncs = _dbcontext.DepartmentPositions
                    .Include(d => d.Department)
                    .Include(p => p.Position)
                    .Where(e => e.DepartmentId.ToString().Contains(searchOption))
                                 .ToList();
                return departmentPositioncs;
            }
            else if (string.IsNullOrEmpty(searchOption) && !string.IsNullOrEmpty(searchValue))
            {
                List<DepartmentPositioncs> departmentPositioncs = _dbcontext.DepartmentPositions
                    .Include(d => d.Department)
                    .Include(p => p.Position)
                    .Where(e => (e.PositionId.ToString().Contains(searchValue)))
                                 .ToList();
                return departmentPositioncs;
            }
            else
            {
                //Return All
                List<DepartmentPositioncs> departmentPositioncs = _dbcontext.DepartmentPositions
                    .Include(d => d.Department)
                    .Include(p => p.Position)
                    .ToList();
                return departmentPositioncs;

            }
        }
    }
}
