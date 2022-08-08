using PlanetZ.Data.Attributes;
using PlanetZ.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Areas.CompanyManager.Models.ViewModels.EmployeeViewModels
{
    public class EditEmployeeVM
    {
        [Required]
        public string Id { get; set; }

        [Required, MaxLength(50)]
        [OnlyLetter]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        [OnlyLetter]
        public string LastName { get; set; }

        [Required, AgeControl]
        public DateTime Birthday { get; set; }

        [Required]
        [StartDateControl]
        public DateTime JobStartDate { get; set; }

        public DateTime? DismissalDate { get; set; }

        [StartDateControl]
        public DateTime? NewJobStartDate { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [Range(0, Int64.MaxValue, ErrorMessage = "Please enter only numbers.")]
        public decimal Salary { get; set; }

        [Required]
        [IdentityControl]
        public string Identity { get; set; }

        [Required]
        [IsPhoneNumberValid]
        public string PhoneNumber { get; set; }

        [Required]
        public EnumRoles Role { get; set; }

        [Required]
        public EnumBloodGroup BloodGroup { get; set; }

        [OnlyLetter]
        [Required]
        public string Profession { get; set; }

        [OnlyLetter]
        [Required]
        public string Title { get; set; }

        [OnlyLetter]
        [Required]
        public string WorkUnit { get; set; }
        public EnumGender Gender { get; set; }

        [IsImageValid(MaxFileSize = 1024)]
        public IFormFile PhotoFile { get; set; }
        public string PhotoPath { get; set; }
        public EnumMaritalStatus MaritalStatus { get; set; }
    }
}
