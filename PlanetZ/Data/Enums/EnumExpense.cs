using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Data.Enums
{
    public enum EnumExpense
    {
        [Display(Name = "Food Expense")]
        FoodExpense,

        [Display(Name = "Accommodation Expense")]
        AccommodationExpense,

        [Display(Name = "Travel Expense")]
        TravelExpense,

        [Display(Name = "Fuel Expense")]
        FuelExpense,

        [Display(Name = "Healtcare Expense")]
        HealthcareExpense

    }
}
