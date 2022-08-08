using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Data.Attributes
{
    public class IdentityControlAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            string identityNumber = (string)value;

            // Length of an identity number of Turkey should be 11 digits.
            if (identityNumber.Length != 11)
                return new ValidationResult("Length of an identity number cannot be longer or shorter than 11");

            // An identity number of Turkey cannot start with 0
            if (identityNumber[0] == '0')
                return new ValidationResult("An identity number cannot start with 0");

            long numbers;

            // An identity number of Turkey only can contain numbers
            if (!long.TryParse(identityNumber, out numbers))
                return new ValidationResult("An identity number only can contain numbers");

            int sumOfOdds = 0;
            int sumOfEvens = 0;

            // Tenth digit of an identity number of Turkey should meet (sumOfOdds * 7 - sumOfEvens) % 10 this mathematical operation.
            // Last digit of an identity number of Turkey should meet (sumOfFirstTenDigits % 10) this mathematical operation.

            for (int i = 0; i <= 8; i += 2)
                sumOfOdds += Convert.ToInt32(identityNumber[i].ToString());

            for (int i = 1; i <= 7; i += 2)
                sumOfEvens += Convert.ToInt32(identityNumber[i].ToString());

            int digitTen = (sumOfOdds * 7 - sumOfEvens) % 10;

            int sumOfFirstTenDigits = 0;

            for (int i = 0; i < identityNumber.Length - 1; i++)
                sumOfFirstTenDigits += Convert.ToInt32(identityNumber[i].ToString());

            int lastDigit = sumOfFirstTenDigits % 10;

            if (digitTen != Convert.ToInt32(identityNumber[9].ToString()) || sumOfFirstTenDigits % 10 != Convert.ToInt32(identityNumber[10].ToString()))
                return new ValidationResult("Invalid identity number");

            return ValidationResult.Success;
        }
    }
}
