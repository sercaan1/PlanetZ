using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Data.Attributes
{
    public class NumberOfUsersAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            int numberOfUsers = (int)value;
            if (numberOfUsers > 100000)
            {
                return new ValidationResult("You have entered too many numbers. ");
            }
            if (numberOfUsers < 3)
            {
                return new ValidationResult("You cannot enter less than '3' numbers.");
            }
            return ValidationResult.Success;

        }
    }
}
