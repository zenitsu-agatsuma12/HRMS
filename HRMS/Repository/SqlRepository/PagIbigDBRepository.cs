using HRMS.Data;
using HRMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Repository.SqlRepository
{
    public class PagIbigDBRepository: IPagIbigRepository
    {
        HRMSDBContext _dbcontext;
        public PagIbigDBRepository(HRMSDBContext dbContext)
        {
            _dbcontext = dbContext;
        }
        public PagIbig AddPagIbig(PagIbig newPagIbig)
        {
            _dbcontext.Add(newPagIbig);
            _dbcontext.SaveChanges();
            return newPagIbig;
        }

        public PagIbig DeletePagIbig(string PagIbigId)
        {
            var Pos = GetPagIbigById(PagIbigId);
            if (Pos != null)
            {
                _dbcontext.pagIbigs.Remove(Pos);
                _dbcontext.SaveChanges();
            }
            return Pos;
        }

        public PagIbig GetPagIbigById(string Id)
        {
            return _dbcontext.pagIbigs.Include(e => e.Employee).AsNoTracking().ToList().FirstOrDefault(x => x.PagIbigId == Id);
        }

        public List<PagIbig> ListOfPagIbig()
        {
            return _dbcontext.pagIbigs.AsNoTracking().ToList();
        }

        public PagIbig UpdatePagIbig(string PagIbigId, PagIbig newPagIbig)
        {
            _dbcontext.pagIbigs.Update(newPagIbig);
            _dbcontext.SaveChanges();
            return newPagIbig;
        }

        public List<SelectListItem> GetEmployeeList()
        {

            var listEmp = new List<SelectListItem>();
            List<Employee> employees = _dbcontext.Employees.ToList();
            listEmp = employees.Select(emp => new SelectListItem
            {
                Value = (emp.EmpId).ToString(),
                Text = emp.FirstName
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "---Select Employee---"
            };
            listEmp.Insert(0, defItem);
            return listEmp;
        }

        public List<PagIbig> ListOfPagIbig(string searchValue)
        {
            if (!string.IsNullOrEmpty(searchValue))
            {
                List<PagIbig> pagIbigs = _dbcontext.pagIbigs
                   .Include(d => d.Employee)
                   .Where(e => e.PagIbigId.Contains(searchValue))
                   .ToList();
                return pagIbigs;
            }

            List<PagIbig> PagIbigs = _dbcontext.pagIbigs
                .Include(d => d.Employee)
                .ToList();
            return PagIbigs;
        }

        /*public PagIbig CreateOrUpdate(int empId, string startdate)
        {
           
           
        }*/

        public PagIbig ConfirmID(int empId, string value)
        {
            PagIbig pagIbig = _dbcontext.pagIbigs.FirstOrDefault(p => p.EmpId == empId);
            bool exists = _dbcontext.pagIbigs.Any(PagIbig => PagIbig.EmpId == empId);
           

            return pagIbig;

        }

        public PagIbig ValueSet(int empId, string startdate)
        {
            PagIbig PagIbig = new PagIbig();
            PagIbig.EmpId = empId;
            PagIbig.StartDate = startdate;

            return PagIbig;
        }

        public PagIbig GetID(int empId)
        {
            var pagIbig = _dbcontext.pagIbigs
           .Where(p => p.EmpId == empId)
           .Select(p => new PagIbig { PagIbigId = p.PagIbigId })
           .FirstOrDefault();

            return pagIbig;
        }
    }
}
