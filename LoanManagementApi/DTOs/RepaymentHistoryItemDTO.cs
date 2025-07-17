using LoanManagementApi.Models.Enums;

namespace LoanManagementApi.DTOs
{
    public class RepaymentHistoryItemDTO
    {
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentStatus Status { get; set; }
        public decimal Penalty { get; set; }
        public List<Guid> RepaymentScheduleIds { get; set; }
    }

}
