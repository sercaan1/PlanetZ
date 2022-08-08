using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Areas.EmployeePage.Models
{
    public class ChangePasswordVM
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        [MaxLength(20)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        [MinLength(8)]
        [MaxLength(20)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
