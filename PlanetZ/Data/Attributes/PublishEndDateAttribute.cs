using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Data.Attributes
{
    public class PublishEndDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            DateTime publishEndDate = (DateTime)value;
            if ((publishEndDate - DateTime.Now).TotalDays > 3650)
            {
                return new ValidationResult("You can enter a date within the next 10 years in this field.");
            }
            if ((publishEndDate - DateTime.Now).TotalDays < 0)
            {
                return new ValidationResult("You cannot enter a date from past.");
            }
            return ValidationResult.Success;

        }
    }
}
