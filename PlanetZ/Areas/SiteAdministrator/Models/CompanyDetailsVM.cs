using PlanetZ.Data.Enums;

namespace PlanetZ.Areas.SiteAdministrator.Models
{
    public class CompanyDetailsVM
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public CompanyType CompanyType { get; set; }
        public string Sector { get; set; }
        public string PhoneNumber { get; set; }
        public string MersisNo { get; set; }
        public DateTime DateOfContract { get; set; }
        public DateTime EndOfContract { get; set; }
        public DateTime FoundedDate { get; set; }
        public string Address { get; set; }
        public string CompanyLogoPath { get; set; }
    }
}
