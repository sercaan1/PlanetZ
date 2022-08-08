using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Data.Attributes
{
    public class FoundatedDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            DateTime foundatedDate = (DateTime)value;
            if ((foundatedDate - DateTime.Now).TotalDays > 0)
            {
                return new ValidationResult("You cannot enter a foundated date from past.");
            }
            if ((DateTime.Now - foundatedDate).TotalDays > 36500)
            {
                return new ValidationResult("You cannot enter a date more than 100 years old.");
            }
            return ValidationResult.Success;

        }
    }
}
