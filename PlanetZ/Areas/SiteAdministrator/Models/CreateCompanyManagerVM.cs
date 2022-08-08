using Microsoft.AspNetCore.Mvc.Rendering;
using PlanetZ.Data;
using PlanetZ.Data.Attributes;
using PlanetZ.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Areas.SiteAdministrator.Models
{
    public class CreateCompanyManagerVM
    {
        [Required(ErrorMessage = "The First Name field is required"), MaxLength(50)]
        [OnlyLetter]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The Last Name field is required"), MaxLength(50)]
        [OnlyLetter]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        [Range(0, Int64.MaxValue, ErrorMessage = "Please enter only numbers.")]
        public decimal Salary { get; set; }

        public List<SelectListItem> Companies { get; set; }

        public int CompanyId { get; set; }

        [Required, AgeControl]
        public DateTime Birthday { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [StartDateControl]
        public DateTime JobStartDate { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [IdentityControl]
        public string Identity { get; set; }

        [Required(ErrorMessage = "The Phone Number field is required")]
        [IsPhoneNumberValid]
        public string PhoneNumber { get; set; }

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
        public EnumMaritalStatus MaritalStatus { get; set; }
    }
}
