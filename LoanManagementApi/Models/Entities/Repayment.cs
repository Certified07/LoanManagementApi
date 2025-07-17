using System.ComponentModel.DataAnnotations;
using LoanManagementApi.Models.Enums;

namespace LoanManagementApi.Models.Entities
{
    public class Repayment
    {
        public string Id { get; set; }
        public string LoanId { get; set; }
        public Loan Loan { get; set; }
        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        [Required]
        public PaymentStatus Status { get; set; }
        [Range(0, double.MaxValue)]
        public decimal Penalty { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<RepaymentSchedule> RepaymentSchedules { get; set; } = new List<RepaymentSchedule>();
    }
}
