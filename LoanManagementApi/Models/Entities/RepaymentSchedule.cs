using System.ComponentModel.DataAnnotations;
using LoanManagementApi.Models.Enums;

namespace LoanManagementApi.Models.Entities
{
    public class RepaymentSchedule
    {
        public string Id { get; set; }
        public string RepaymentId { get; set; }
        public Repayment Repayment { get; set; }

        public DateTime DueDate { get; set; }
        public decimal AmountDue { get; set; }

        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        public DateTime? PaymentDate { get; set; }
        public decimal Penalty { get; set; } = 0;
    }

}
