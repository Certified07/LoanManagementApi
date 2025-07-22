namespace LoanManagementApi.RequestModel
{
    public class MakeRepaymentRequestModel
    {
        public string loanId { get; set; }
        public decimal Amount { get; set; }
    }
}
