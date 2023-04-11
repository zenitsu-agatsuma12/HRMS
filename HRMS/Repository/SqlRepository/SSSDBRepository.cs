using HRMS.Data;
using HRMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Repository.SqlRepository
{
    public class SSSDBRepository : ISSSRepository
    {
        HRMSDBContext _dbcontext;
        public SSSDBRepository(HRMSDBContext dbContext)
        {
            _dbcontext = dbContext;
        }
        public SSS AddSSS(SSS newSSS)
        {
            _dbcontext.Add(newSSS);
            _dbcontext.SaveChanges();
            return newSSS;
        }

        public SSS DeleteSSS(string SSSId)
        {
            var Pos = GetSSSById(SSSId);
            if (Pos != null)
            {
                _dbcontext.SSSs.Remove(Pos);
                _dbcontext.SaveChanges();
            }
            return Pos;
        }

        public SSS GetSSSById(string Id)
        {
            return _dbcontext.SSSs.Include(e => e.Employee).AsNoTracking().ToList().FirstOrDefault(x => x.SSSNumber == Id);
        }

        public List<SSS> ListOfSSS()
        {
            return _dbcontext.SSSs.AsNoTracking().ToList();
        }

        public SSS UpdateSSS(string SSSId, SSS newSSS)
        {
            _dbcontext.SSSs.Update(newSSS);
            _dbcontext.SaveChanges();
            return newSSS;
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

        public List<SSS> ListOfSSS(string searchValue)
        {
            if (!string.IsNullOrEmpty(searchValue))
            {
                List<SSS> SSSs = _dbcontext.SSSs
                   .Include(d => d.Employee)
                   .Where(e => e.SSSNumber.Contains(searchValue))
                   .ToList();
                return SSSs;
            }

            List<SSS> SSSss = _dbcontext.SSSs
                .Include(d => d.Employee)
                .ToList();
            return SSSss;
        }

        /*public SSS CreateOrUpdate(int empId, string startdate)
        {
           
           
        }*/

        public SSS ConfirmID(int empId, string value)
        {
            SSS SSS = _dbcontext.SSSs.FirstOrDefault(p => p.EmpId == empId);
            bool exists = _dbcontext.SSSs.Any(SSS => SSS.EmpId == empId);


            return SSS;

        }

        public SSS ValueSet(int empId, string startdate)
        {
            SSS SSS = new SSS();
            SSS.EmpId = empId;
            SSS.StartDate = startdate;

            return SSS;
        }

        public SSS GetID(int empId)
        {
            var SSS = _dbcontext.SSSs
           .Where(p => p.EmpId == empId)
           .Select(p => new SSS { SSSNumber = p.SSSNumber })
           .FirstOrDefault();

            return SSS;
        }
    }
}
