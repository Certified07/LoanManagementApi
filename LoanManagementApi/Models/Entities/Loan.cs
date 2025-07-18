using System.ComponentModel.DataAnnotations;
using LoanManagementApi.Models.Enums;

namespace LoanManagementApi.Models.Entities
{
    public class Loan
    {
        public string Id { get; set; }
        public string ClientId { get; set; }
        public Client Client { get; set; }
        public decimal Amount { get; set; }
        public decimal InterestRate { get; set; }
        public int DurationInMonths { get; set; }
        public LoanStatus Status { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Repayment Repayment { get; set; }
    }
}
