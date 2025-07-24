using LoanManagementApi.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanManagementApi.Models.Entities
{
    public class Loan
    {
        public string Id { get; set; }
        public string ClientId { get; set; }
        public Client Client { get; set; }
        [Column(TypeName = "decimal(22,2)")]
        public decimal PrincipalAmount { get; set; }
        [Column(TypeName = "decimal(22,2)")]
        public decimal InterestRate { get; set; }
        [Column(TypeName = "decimal(22,2)")]
        public decimal TotalAmountToRepay { get; set; }
        public int DurationInMonths { get; set; }
        public LoanStatus Status { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public Repayment? Repayment { get; set; }
        [Column(TypeName = "decimal(22,2)")]
        public decimal TotalPaid { get; set; } = 0;
        public bool IsCompleted { get; set; }
        public RepaymentType RepaymentType { get; set; }
        public int LoanTypeId { get; set; }
        public LoanType LoanType { get; set; }

    }
}
