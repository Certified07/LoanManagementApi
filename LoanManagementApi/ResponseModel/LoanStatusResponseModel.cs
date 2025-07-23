namespace LoanManagementApi.ResponseModel
{
    public class LoanStatusResponseModel : BaseResponse
    {
        public string LoanId { get; set; }
        public string ClientId { get; set; }
        public string LoanStatus { get; set; }
        public int TotalPayment { get; set; }
    }
}
