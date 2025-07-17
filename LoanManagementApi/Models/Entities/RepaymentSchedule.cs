using System.ComponentModel.DataAnnotations;
using LoanManagementApi.Models.Enums;

namespace LoanManagementApi.Models.Entities
{
    public class RepaymentSchedule
    {
        public string Id { get; set; }
        public string LoanId { get; set; }
        public Loan Loan { get; set; }
        public string? RepaymentId { get; set; }
        public Repayment Repayment { get; set; }
        public int InstallmentNumber { get; set; }
        public DateTime DueDate { get; set; }
        [Range(0, double.MaxValue)]
        public decimal AmountDue { get; set; }
        [Required]
        public PaymentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
