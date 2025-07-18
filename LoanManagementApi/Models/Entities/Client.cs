using System.ComponentModel.DataAnnotations;

namespace LoanManagementApi.Models.Entities
{
    public class Client
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int CreditScore { get; set; }
        public decimal Income { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
