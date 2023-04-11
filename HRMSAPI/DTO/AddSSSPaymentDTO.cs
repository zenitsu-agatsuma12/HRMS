namespace HRMSAPI.DTO
{
    public class AddSSSPaymentDTO
    {
        public string SSSNumber { get; set; }
        public int Payment { get; set; }
        public string Month { get; set; } = DateTime.Now.Month.ToString();
        public string Year { get; set; } = DateTime.Now.Year.ToString();
    }
}
