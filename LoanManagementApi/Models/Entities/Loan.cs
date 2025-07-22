using System.ComponentModel.DataAnnotations;
using LoanManagementApi.Models.Enums;

namespace LoanManagementApi.Models.Entities
{
    public class Loan
    {
        public string Id { get; set; }
        public string ClientId { get; set; }
        public Client Client { get; set; }
        public decimal PrincipalAmount { get; set; }
        public decimal InterestRate { get; set; }
        public decimal TotalAmountToRepay { get; set; }
        public int DurationInMonths { get; set; }
        public LoanStatus Status { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public Repayment? Repayment { get; set; }
        public decimal TotalPaid { get; set; } = 0;
        public bool IsCompleted { get; set; }
        public RepaymentType RepaymentType { get; set; }
        public int LoanTypeId { get; set; }
        public LoanType LoanType { get; set; }

    }
}
