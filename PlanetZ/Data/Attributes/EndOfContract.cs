using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Data.Attributes
{
    public class EndOfContract : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            DateTime endofContract = (DateTime)value;
            if ((endofContract - DateTime.Now).TotalDays > 3650)
            {
                return new ValidationResult("You can enter a date within the next 10 years in this field.");
            }
            if ((endofContract - DateTime.Now).TotalDays < 0)
            {
                return new ValidationResult("You cannot enter a date from past.");
            }
            return ValidationResult.Success;

        }
    }
}
