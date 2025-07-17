using System.ComponentModel.DataAnnotations;

namespace LoanManagementApi.DTOs
{
    public class ClientCreateDTO
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Phone { get; set; }
        [Range(0, 1000)]
        public int CreditScore { get; set; }
        [Range(0, double.MaxValue)]
        public decimal Income { get; set; }
    }
}
