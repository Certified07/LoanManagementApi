using System.ComponentModel.DataAnnotations;

namespace LoanManagementApi.Models.Entities
{
    public class Client
    {
        public string Id { get; set; }
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
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
