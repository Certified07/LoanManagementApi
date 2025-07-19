namespace LoanManagementApi.RequestModel
{
    public class MakeRepaymentRequestModel
    {
        public string ScheduleId { get; set; }
        public decimal Amount { get; set; }
    }
}
