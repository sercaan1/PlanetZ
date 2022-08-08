using PlanetZ.Data.Attributes;
using PlanetZ.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Areas.SiteAdministrator.Models
{
    public class CreateCompanyVM
    {
        [Required(ErrorMessage = "The Company Name field is required"), MaxLength(100)]
        [OnlyLetter]
        public string CompanyName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Adress")]
        [Required(ErrorMessage = "The Email field is required"), MaxLength(100)]
        public string Email { get; set; }

        [Required(ErrorMessage = "The Phone Number field is required")]
        [IsPhoneNumberValid]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "The Mersis field is required")]
        [Range(0, Int64.MaxValue, ErrorMessage = "Please enter only numbers.")]
        public string MersisNo { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DateOfContract]
        public DateTime DateOfContract { get; set; }
        
        [Required(ErrorMessage = "This field is required")]
        [EndOfContract]
        public DateTime EndOfContract { get; set; }

        public CompanyType CompanyType { get; set; }


        [Required(ErrorMessage = "This field is required")]
        [FoundatedDate]
        public DateTime FoundedDate { get; set; }

        [Required(ErrorMessage = "The Address field is required"), MaxLength(200)]
        public string Address { get; set; }

        [Required(ErrorMessage = "The Sector field is required"), MaxLength(200)]
        public EnumSectors Sector { get; set; }

        public IFormFile PhotoFile { get; set; }

    }
}
