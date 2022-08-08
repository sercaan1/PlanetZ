using PlanetZ.Data.Attributes;
using PlanetZ.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Areas.EmployeePage.Models
{
    public class AdvanceRequestVM
    {
        public EnumAdvanceType AdvanceType { get; set; }

        [Required]
        [AdvanceRequest]
        public decimal AdvanceAmount { get; set; }

        public EnumCurrencyUnit AdvanceCurrency { get; set; }

        [Required]
        public string AdvanceDescription { get; set; }
    }
}
