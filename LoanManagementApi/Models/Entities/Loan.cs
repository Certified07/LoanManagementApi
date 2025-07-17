using System.ComponentModel.DataAnnotations;
using LoanManagementApi.Models.Enums;

namespace LoanManagementApi.Models.Entities
{
    public class Loan
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public Client Client { get; set; }
        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }
        [Range(0, 100)]
        public decimal InterestRate { get; set; }
        [Range(1, 360)]
        public int TermMonths { get; set; }
        [Required]
        public LoanStatus Status { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
