using System.ComponentModel.DataAnnotations;

namespace LoanManagementApi.DTOs
{
    public class LoanApprovalDTO
    {
        [Required]
        public Guid AdminId { get; set; }
        [Range(0, double.MaxValue)]
        public decimal ApprovedAmount { get; set; }
        [Range(0, 100)]
        public decimal InterestRate { get; set; }
        [Range(1, 360)]
        public int TermMonths { get; set; }
    }

}
