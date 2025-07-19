namespace LoanManagementApi.DTOs
{
    public class RepaymentScheduleDTO
    {
        public string ScheduleId { get; set; }
        public string DueDate { get; set; }
        public decimal Amount { get; set; }
        public decimal Penalty { get; set; }
        public decimal AmountPaid { get; set; }
        public string Status { get; set; }
    }
}
