using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Data.Attributes
{
    public class DateOfContractAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            DateTime dateofContract = (DateTime)value;
            if ((DateTime.Now - dateofContract).TotalDays > 3650)
            {
                return new ValidationResult("You cannot enter a date earlier than 10 years.");
            }
            if ((DateTime.Now - dateofContract).TotalDays < 0)
            {
                return new ValidationResult("You cannot enter a date from future.");
            }
            return ValidationResult.Success;

        }
    }
}
