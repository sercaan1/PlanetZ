using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Data.Attributes
{
    public class PublishStartDayAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            DateTime publishStartDay = (DateTime)value;
            if ((publishStartDay - DateTime.Now).TotalDays > 365)
            {
                return new ValidationResult("You can enter this field in just 1 year.");
            }
            if ((publishStartDay - DateTime.Now).TotalDays < 0)
            {
                return new ValidationResult("You cannot enter a date from past.");
            }
            return ValidationResult.Success;

        }
    }
}
