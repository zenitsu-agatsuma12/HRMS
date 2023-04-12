using HRMSAPI.Data;
using HRMSAPI.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HRMSAPI.Repository.SqlRepository
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

        public List<SSSPayment> ListOfSSSPayment()
        {
            return _dbcontext.SSSPayments.ToList();
        }

        public SSSPayment UpdateSSSPayment( SSSPayment newSSSPayment, int no)
        {
            _dbcontext.SSSPayments.Update(newSSSPayment);
            _dbcontext.SaveChanges();
            return newSSSPayment;
        }


    }
}
