using System.ComponentModel.DataAnnotations;

namespace LoanManagementApi.RequestModel
{
    public class RegisterUserRequestModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        public decimal Income { get; set; }
        [Phone]
        public string Phone { get; set; }
    }
}
