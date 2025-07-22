using LoanManagementApi.Validators;
using System.ComponentModel.DataAnnotations;

namespace LoanManagementApi.RequestModel
{
    public class RegisterUserRequestModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required]
        //[RegularExpression(@"^[a-zA-Z0-9]{6,}$", ErrorMessage = "Password must be at least 6 alphanumeric characters")]
        public string Password { get; set; }
        [DateOfBirthValidation(ErrorMessage = "Invalid date of birth.")]
        public DateOnly DateOfBirth { get; set; }
        public decimal Income { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^0\d{10}$", ErrorMessage = "Phone number must start with 0 and be exactly 11 digits.")]
        public string PhoneNumber { get; set; }
    }
}
