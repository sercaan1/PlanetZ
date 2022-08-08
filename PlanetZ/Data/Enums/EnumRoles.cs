using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Data.Enums
{
    public enum EnumRoles
    {
        Employee = 1,
        [Display(Name = "Company Manager")]
        CompanyManager,
        [Display(Name = "Site Manager")]
        SiteManager
    }
}
