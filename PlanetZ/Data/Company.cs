using PlanetZ.Data.Attributes;
using PlanetZ.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Data
{
    public class Company
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string CompanyName { get; set; }
        [EmailAddress(ErrorMessage ="Invalid Email Adress")]
        public string Email { get; set; }
        [IsPhoneNumberValid]
        public string PhoneNumber { get; set; }
        [Required]
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

        [Required, MaxLength(200)]
        public string Address { get; set; }
        [Required]
        public EnumSectors Sector { get; set; }

        public string CompanyLogoPath { get; set; }

        public List<Employee> Employees { get; set; }

        public List<Package> Packages { get; set; } = new List<Package>();

        public int WalletId { get; set; }

        public Wallet Wallet { get; set; }
    }
}
