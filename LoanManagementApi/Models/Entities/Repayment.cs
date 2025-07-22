

using LoanManagementApi.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanManagementApi.Models.Entities
{
    public class Repayment
    {
        public string Id { get; set; }
        public string LoanId { get; set; }
        public Loan Loan { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal AmountPaid {  get; set; }

        public DateTime PaymentDate { get; set; }

        public PaymentStatus Status { get; set; }

        public decimal Penalty { get; set; } = 0;

        public DateTime CreatedAt { get; set; }

        public ICollection<RepaymentSchedule> RepaymentSchedules { get; set; }
    }
}
