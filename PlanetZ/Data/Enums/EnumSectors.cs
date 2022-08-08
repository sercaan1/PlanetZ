using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Data.Enums
{
    public enum EnumSectors
    {
        Agriculture,
        Chemical,
        Commerce,
        Construction,
        Education,
        [Display(Name = "Financial Services")]
        FinancialServices,
        Food,
        [Display(Name = "Health Services")]
        HealthServices,
        Tourism,
        Mining,
        Engineering,
        Media,
        [Display(Name = "Telecommunication Services")]
        TelecommunicatiosServices,
        Textile,
        Transport
    }
}
