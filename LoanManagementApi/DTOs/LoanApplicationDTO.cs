using System.ComponentModel.DataAnnotations;

namespace LoanManagementApi.DTOs
{
    public class LoanApplicationDTO
    {
        [Required]
        public Guid ClientId { get; set; }
        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }
        [Range(1, 360)]
        public int TermMonths { get; set; }
        [Range(0, 100)]
        public decimal? InterestRate { get; set; }
    }
}
