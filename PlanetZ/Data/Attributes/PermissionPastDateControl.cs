using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Data.Attributes
{
    public class PermissionPastDateControl : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            DateTime date = (DateTime)value;

            if (DateTime.Now > date)
                return new ValidationResult("You cannot choose past days.");

            if (date.Year > DateTime.Now.Year)
            {
                return new ValidationResult("You can request permission only for this year.");
            }

            return ValidationResult.Success;
        }
    }
}
