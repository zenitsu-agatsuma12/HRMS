using HRMSAPI.Data;
using HRMSAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMSAPI.Repository.SqlRepository
{
    public class PagIbigPaymentDBRepository : IPagIbigPaymentRepository
    {
        HRMSDBContext _dbcontext;

        public PagIbigPaymentDBRepository (HRMSDBContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public PagIbigPayment AddPagIbigPayment(PagIbigPayment newPagIbigPayment)
        {
            _dbcontext.Add(newPagIbigPayment);
            _dbcontext.SaveChanges();
            return newPagIbigPayment;
        }

        public PagIbigPayment DeletePagIbigPayment(int No)
        {
            PagIbigPayment pagIbigPayment = GetPagIbigPaymentById(No);
            if (pagIbigPayment != null)
            {
                _dbcontext.Remove(pagIbigPayment);
                _dbcontext.SaveChanges();
            }
            return pagIbigPayment;
        }
        public PagIbigPayment GetPagIbigPaymentById(int No)
        {
            return _dbcontext.PagIbigPayments.AsNoTracking().ToList().FirstOrDefault(x =>x.No == No);
        }

        public List<PagIbigPayment> ListOfPagIbigPayment()
        {
            return _dbcontext.PagIbigPayments.ToList();
        }

        public PagIbigPayment UpdatePagIbigPayment(PagIbigPayment newPagIbigPayment)
        {
            _dbcontext.Update(newPagIbigPayment);
            _dbcontext.SaveChanges() ;
            return newPagIbigPayment;
        }
    }
}
