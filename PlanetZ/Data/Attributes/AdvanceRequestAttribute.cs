using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Data.Attributes
{
    public class AdvanceRequestAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            var httpContextAccessor = validationContext.GetRequiredService<IHttpContextAccessor>();
            var db = validationContext.GetRequiredService<ApplicationDbContext>();
            var id = httpContextAccessor.HttpContext.Session.GetString("userId");
            var user = db.Users.Include(x => x.Advances).FirstOrDefault(x => x.Id == id);

            decimal totalAdvances = 0;
            if (user.Advances != null)
            {
                foreach (var item in user.Advances.Where(x => x.AdvanceType == Enums.EnumAdvanceType.AdvancePayment))
                {
                    totalAdvances += item.AdvanceAmount;
                }
            }

            var advance = (decimal)value;

            if (advance <= 0)
                return new ValidationResult("The number entered must be greater than 0.");

            if (advance + totalAdvances > user.Salary * 3.6m)
                return new ValidationResult($"Your total advance request cannot be higher than 30% of your annual salary. You already requested ₺{totalAdvances}.");

            return ValidationResult.Success;
        }
    }
}
