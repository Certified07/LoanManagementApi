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
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{6,}$",
    ErrorMessage = "Password must be at least 6 characters long and include at least 1 uppercase letter, 1 lowercase letter, 1 number, and 1 special character.")]
        public string Password { get; set; }
        [DateOfBirthValidation(ErrorMessage = "Invalid date of birth.")]
        public DateOnly DateOfBirth { get; set; }
        public decimal Income { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^0\d{10}$", ErrorMessage = "Phone number must start with 0 and be exactly 11 digits.")]
        public string PhoneNumber { get; set; }
    }
}
