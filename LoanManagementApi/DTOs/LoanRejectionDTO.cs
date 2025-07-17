using System.ComponentModel.DataAnnotations;

namespace LoanManagementApi.DTOs
{
    public class LoanRejectionDTO
    {
        [Required]
        public Guid AdminId { get; set; }
        [Required]
        public string Reason { get; set; }
    }
    
}
