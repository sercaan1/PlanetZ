using PlanetZ.Data.Enums;

namespace PlanetZ.Areas.EmployeePage.Models
{
    public class ListAdvanceVM
    {
        public string EmployeeFullName { get; set; }

        public decimal AdvanceAmount { get; set; }

        public string AdvanceDescription { get; set; }

        public DateTime RequestDate { get; set; }

        public DateTime? ReplyDate { get; set; }

        public EnumAdvanceStatus AdvanceStatus { get; set; }

        public EnumCurrencyUnit AdvanceCurrency { get; set; }

        public EnumAdvanceType AdvanceType { get; set; }

        public string ReturnResponse { get; set; }
    }
}
