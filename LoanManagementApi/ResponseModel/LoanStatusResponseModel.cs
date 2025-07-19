namespace LoanManagementApi.ResponseModel
{
    public class LoanStatusResponseModel : BaseResponse
    {
        public string LoanId { get; set; }
        public string ClientId { get; set; }
        public string LoanStatus { get; set; }
        public int MissedPayments { get; set; }
        public DateTime? LastPaymentDate { get; set; }
        public decimal TotalOutstanding { get; set; }
    }
}
