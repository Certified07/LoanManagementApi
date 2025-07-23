namespace LoanManagementApi.RequestModel
{
    public class LoanRequestModel
    {
        public decimal Amount { get; set; }
        public string LoanType { get; set; }
        public string Purpose { get; set; }
        public int DurationInMonths { get; set; }

    }
}
