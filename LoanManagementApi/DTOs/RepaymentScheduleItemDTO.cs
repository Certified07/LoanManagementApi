using LoanManagementApi.Models.Enums;

namespace LoanManagementApi.DTOs
{
    public class RepaymentScheduleItemDTO
    {
        public Guid Id { get; set; }
        public int InstallmentNumber { get; set; }
        public DateTime DueDate { get; set; }
        public decimal AmountDue { get; set; }
        public PaymentStatus Status { get; set; }
        public Guid? RepaymentId { get; set; }
    }

}
