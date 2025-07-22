using LoanManagementApi.DTOs;

namespace LoanManagementApi.ResponseModel
{
    public class RepaymentResponseModel
    {
        public string RepaymentId { get; set; }
        public string LoanId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal AmountPaid { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PaymentDate { get; set; }
        public List<RepaymentScheduleDTO> Schedules { get; set; }
    }
}
