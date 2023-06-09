﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMSAPI.Models
{
    public class PagIbigPayment
    {
        [Key]
        public int No { get; set; }
        public string FullName { get; set; }
        public string? PagIbigNumber { get; set; }

        public int Payment { get; set; }
        public string Month { get; set; } = DateTime.Now.Month.ToString();
        public string Year { get; set; } = DateTime.Now.Year.ToString();

        public bool status { get; set; }

        public PagIbigPayment() { }

        public PagIbigPayment(int no, string? pagIbigNumber, int payment, string month, string year, bool status)
        {
            No = no;
            PagIbigNumber = pagIbigNumber;
            Payment = payment;
            Month = month;
            Year = year;
            this.status = status;
        }
    }
}
