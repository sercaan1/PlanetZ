using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Data.Attributes
{
    public class ExpenseMinAndMaxControl : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            decimal price = (decimal)value;

            if (price == 0)
            {
                ErrorMessage = "You cannot request for amount of 0";
                return false;
            }

            if (price > 50000)
            {
                ErrorMessage = "Maximum amount of one time request is ₺50000";
                return false;
            }

            return true;
        }
    }
}
