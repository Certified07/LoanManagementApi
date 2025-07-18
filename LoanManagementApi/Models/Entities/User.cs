using System.ComponentModel.DataAnnotations;
using LoanManagementApi.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace LoanManagementApi.Models.Entities
{
    public class MyUser : IdentityUser
    {
        public string Id { get; set; }
        [Required]
        public UserRole Role { get; set; }
    }
}
