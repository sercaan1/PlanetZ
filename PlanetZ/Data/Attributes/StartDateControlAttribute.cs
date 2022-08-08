using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Data.Attributes
{
    public class StartDateControlAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            DateTime date = (DateTime)value;

            if (date > DateTime.Now.AddMonths(1))
                return new ValidationResult("Employee can start within a month at the latest.");

            int totalYears = DateTime.Now.Year - date.Year;

            if (totalYears > 60)
                return new ValidationResult("It's time for the employee to retire?!");

            return ValidationResult.Success;
        }
    }
}
