using PlanetZ.Data.Attributes;
using PlanetZ.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Areas.EmployeePage.Models
{
    public class CreatePermissionVM
    {
        [PermissionPastDateControl]
        [Required]
        public DateTime PermitStartDate { get; set; }

        [PermissionPastDateControl]
        public DateTime? LeaveExpiryDate { get; set; }

        public int? Duration { get; set; }

        public EnumPermission PermitType { get; set; }
    }
}
