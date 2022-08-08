using PlanetZ.Data.Attributes;
using PlanetZ.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Areas.SiteAdministrator.Models
{
    public class EditCompanyVM
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string CompanyName { get; set; }
        [EmailAddress] 
        public string Email { get; set; }
        [IsPhoneNumberValid]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [EndOfContract]
        public DateTime ContractEndDate { get; set; }
        public CompanyType CompanyType { get; set; }

        [Required, MaxLength(200)]
        public string Address { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public EnumSectors Sector { get; set; }
        public IFormFile PhotoFile { get; set; }
        public string CompanyLogoPath { get; set; }

    }
}
