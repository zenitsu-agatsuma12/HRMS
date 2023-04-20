using HRMS.Data;
using HRMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Repository.SqlRepository
{
    public class SSSPaymentDBRepository : ISSSPaymentRepository
    {
        HRMSDBContext _dbcontext;
        public SSSPaymentDBRepository(HRMSDBContext dbContext)
        {
            _dbcontext = dbContext;
        }

        public SSSPayment AddSSSPayment(SSSPayment newSSSPayment)
        {
            _dbcontext.Add(newSSSPayment);
            _dbcontext.SaveChanges();
            return newSSSPayment;
        }

        public SSSPayment DeleteSSSPayment(int No)
        {
            SSSPayment sssPayment = GetSSSPaymentById(No);
            if (sssPayment != null)
            {
                _dbcontext.SSSPayments.Remove(sssPayment);
                _dbcontext.SaveChanges();
            }
                return sssPayment;
        }

        public SSSPayment GetSSSPaymentById(int No)
        {
            return _dbcontext.SSSPayments.ToList().FirstOrDefault(x => x.No == No);

        }

        public List<SSSPayment> ListOfSSSPayment(string searchValue)
        {
            IQueryable<SSSPayment> payments = _dbcontext.SSSPayments;

            if (!string.IsNullOrEmpty(searchValue))
            {
                payments = payments.Where(p => p.FullName.Contains(searchValue) ||
                                                p.Payment.ToString().Contains(searchValue) || 
                                                p.Month.Contains(searchValue) ||
                                                p.Year.Contains(searchValue));
            }

            var model = payments.ToList();
            return model;
        }

        public SSSPayment UpdateSSSPayment( SSSPayment newSSSPayment)
        {
            _dbcontext.SSSPayments.Update(newSSSPayment);
            _dbcontext.SaveChanges();
            return newSSSPayment;
        }

    }
}
