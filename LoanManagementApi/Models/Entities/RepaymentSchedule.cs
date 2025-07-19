using LoanManagementApi.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanManagementApi.Models.Entities
{
    public class RepaymentSchedule
    {
        public string Id { get; set; }

        public string RepaymentId { get; set; }
        public Repayment Repayment { get; set; }

        public DateTime DueDate { get; set; }

        public decimal Amount { get; set; } 

        public decimal AmountPaid { get; set; } = 0;

        public decimal Penalty { get; set; } = 0;

        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        public bool IsPaid => AmountPaid >= Amount + Penalty;

        public DateTime? PaymentDate { get; set; }

        public DateTime? LastPenaltyCalculationDate { get; set; }
    }

}
