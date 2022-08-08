using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Data.Enums
{
    public enum EnumBloodGroup
    {
        [Display(Name = "A+")]
        APos = 1,

        [Display(Name = "A-")]
        ANeg,

        [Display(Name = "B+")]
        BPos,

        [Display(Name = "B-")]
        BNeg,

        [Display(Name = "AB+")]
        ABPos,

        [Display(Name = "AB-")]
        ABNeg,

        [Display(Name = "0+")]
        OPos,

        [Display(Name = "0-")]
        ONeg
    }
}
