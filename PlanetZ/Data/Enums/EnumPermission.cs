using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Data.Enums
{
    public enum EnumPermission
    {
        [Display(Name = "Annual Leave")]
        AnnualPermit,

        [Display(Name = "Excuse Leave")]
        ExcuseLeave,

        [Display(Name = "Administrative Leave")]
        AdministrativeLeave,

        [Display(Name = "Paternity Leave")]
        PaternityLeave,

        [Display(Name = "Marriage Permission")]
        MarriagePermission
    }
}
