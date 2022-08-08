using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Data.Attributes
{
    public class IsPhoneNumberValidAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            string phoneNumber = (string)value;

            if (phoneNumber.Length < 14 || phoneNumber.Length > 14)
                return new ValidationResult("Invalid phone number. Phone number should contain 10 digits (5554443322)");

            string firstDigit = phoneNumber[1].ToString();

            if (firstDigit != "5")
                return new ValidationResult("Invalid phone number. Phone number starts with \"5\"");

            return ValidationResult.Success;
        }
    }
}
