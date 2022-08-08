using PlanetZ.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Data
{
    public class Advance
    {
        public int Id { get; set; }

        [Required]
        public decimal AdvanceAmount { get; set; }

        [Required, MaxLength(100)]
        public string AdvanceDescription { get; set; }

        public DateTime RequestDate { get; set; } = DateTime.Now;

        public DateTime? ReplyDate { get; set; }

        public EnumAdvanceStatus AdvanceStatus { get; set; }

        public EnumCurrencyUnit AdvanceCurrency { get; set; }

        public EnumAdvanceType AdvanceType { get; set; }

        public string ReturnResponse { get; set; }

        public string EmployeeId { get; set; }

        public Employee Employee { get; set; }
    }
}
