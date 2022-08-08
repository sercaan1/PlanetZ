using PlanetZ.Data.Attributes;
using PlanetZ.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Data
{
    public class Permission
    {
        public int Id { get; set; }

        [PermissionPastDateControl]
        [Required]
        public DateTime PermitStartDate { get; set; }

        [PermissionPastDateControl]
        public DateTime? LeaveExpiryDate { get; set; }

        public DateTime? ReplyDate { get; set; }

        public int? Duration { get; set; }

        public EnumPermission PermitType { get; set; }
        public EnumPermissionStatus PermissionStatus { get; set; }

        public string EmployeeId { get; set; }
        public Employee Employee { get; set; }

    }
}
