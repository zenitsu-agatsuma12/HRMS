namespace HRMSAPI.DTO
{
    public class AddPagIbigPaymentDTO
    {
        public string PagIbigNumber { get; set; }
        public int Payment { get; set; }
        public string Month { get; set; } = DateTime.Now.Month.ToString();
        public string Year { get; set; } = DateTime.Now.Year.ToString();
    }
}
