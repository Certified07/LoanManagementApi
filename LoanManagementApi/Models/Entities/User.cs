using System.ComponentModel.DataAnnotations;
using LoanManagementApi.Models.Enums;

namespace LoanManagementApi.Models.Entities
{
    public class User
    {
        public string Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        [Required]
        public UserRole Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
