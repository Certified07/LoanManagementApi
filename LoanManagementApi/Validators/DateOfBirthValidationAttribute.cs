using System.ComponentModel.DataAnnotations;

namespace LoanManagementApi.Validators
{
    public class DateOfBirthValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is not DateOnly dob)
                return false;

            var today = DateOnly.FromDateTime(DateTime.Today);

            return dob < today;
        }
    }

}
