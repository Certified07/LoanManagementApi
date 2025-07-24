using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanManagementApi.Models.Entities
{
    public class Client
    {
        public string Id { get; set; }
        public MyUser User { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int CreditScore { get; set; }
        [Column(TypeName = "decimal(22,2)")]
        public decimal Income { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }
}
