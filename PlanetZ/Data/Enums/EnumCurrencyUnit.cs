using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Data.Enums
{
    public enum EnumCurrencyUnit
    {
        [Display(Name = "TL")]
        TL = 1,
        
        USD,

        EUR
    }
}
