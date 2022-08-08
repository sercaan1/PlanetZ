using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Data.Attributes
{
    public class AgeControlAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            DateTime birthday = (DateTime)value;

            if ((DateTime.Now - birthday).TotalDays < 6574)
            {
                return new ValidationResult("You cannot have employees under the age of 18");
            }
            if ((DateTime.Now - birthday).TotalDays > 23741)
            {
                return new ValidationResult("You cannot have employees over the age of 65");
            }

            return ValidationResult.Success;
        }
    }
}
