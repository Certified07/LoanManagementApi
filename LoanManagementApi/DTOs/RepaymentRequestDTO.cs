using System.ComponentModel.DataAnnotations;

namespace LoanManagementApi.DTOs
{
    public class RepaymentRequestDTO
    {
        [Required]
        public Guid LoanId { get; set; }
        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public List<Guid> RepaymentScheduleIds { get; set; }
    }
}
