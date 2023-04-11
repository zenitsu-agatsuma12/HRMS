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

        public List<PhilHealthPayment> ListOfPhilHealthPayment()
        {
           return _dbcontext.PhilHealthPayments.ToList();
        }

        public PhilHealthPayment UpdatePhilHealthPayment(PhilHealthPayment newPhilHealthPayment)
        {
           _dbcontext.Update(newPhilHealthPayment);
           _dbcontext.SaveChanges();
           return newPhilHealthPayment;
        }
    }
}
