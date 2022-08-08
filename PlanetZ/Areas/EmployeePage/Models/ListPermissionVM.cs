using PlanetZ.Data.Enums;

namespace PlanetZ.Areas.EmployeePage.Models
{
    public class ListPermissionVM
    {
        public DateTime PermitStartDate { get; set; }

        public DateTime? LeaveExpiryDate { get; set; }

        public DateTime? ReplyDate { get; set; }

        public int? Duration { get; set; }

        public EnumPermission PermitType { get; set; }
        public EnumPermissionStatus PermissionStatus { get; set; }
        public string EmployeeFullName { get; set; }
    }
}
