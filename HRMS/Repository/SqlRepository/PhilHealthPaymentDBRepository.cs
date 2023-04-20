using HRMS.Data;
using HRMS.Models;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Repository.SqlRepository
{
    public class PhilHealthPaymentDBRepository : IPhilHealthPaymentDBRepository
    {
        HRMSDBContext _dbcontext;

        public PhilHealthPaymentDBRepository (HRMSDBContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public PhilHealthPayment AddPhilHealthPayment(PhilHealthPayment newPhilHealthPayment)
        {
            _dbcontext.Add(newPhilHealthPayment);
            _dbcontext.SaveChanges();
            return newPhilHealthPayment;
        }

        public PhilHealthPayment DeletePhilHealthPayment(int No)
        {
            PhilHealthPayment philHealthPayment = GetPhilHealthPaymentById(No);
            if (philHealthPayment != null)
            {
                _dbcontext.Remove(philHealthPayment);
                _dbcontext.SaveChanges();
            }
            return philHealthPayment;
        }

        public PhilHealthPayment GetPhilHealthPaymentById(int No)
        {
            return _dbcontext.PhilHealthPayments.AsNoTracking().ToList().FirstOrDefault(x => x.No == No);
        }

        public List<PhilHealthPayment> ListOfPhilHealthPayment(string searchValue)
        {
            IQueryable<PhilHealthPayment> payments = _dbcontext.PhilHealthPayments;

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

        public PhilHealthPayment UpdatePhilHealthPayment(PhilHealthPayment newPhilHealthPayment)
        {
           _dbcontext.Update(newPhilHealthPayment);
           _dbcontext.SaveChanges();
           return newPhilHealthPayment;
        }
    }
}
