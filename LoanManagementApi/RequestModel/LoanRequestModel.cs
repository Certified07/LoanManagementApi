namespace LoanManagementApi.RequestModel
{
    public class LoanRequestModel
    {
        public string ClientId { get; set; }
        public decimal Amount { get; set; }
        public int LoanId { get; set; }
        public string Purpose { get; set; }
        public int DurationInMonths { get; set; }

    }
}
