using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Data.Attributes
{
    public class OnlyLetterAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            string name = (string)value;

            for (int i = 0; i < name.Length; i++)
            {
                if (char.IsDigit(name[i]))
                    return new ValidationResult("This field cannot contain numbers");
            }
            return ValidationResult.Success;
        }
    }
}
