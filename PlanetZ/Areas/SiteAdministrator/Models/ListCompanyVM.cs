using PlanetZ.Data.Enums;

namespace PlanetZ.Areas.SiteAdministrator.Models
{
    public class ListCompanyVM
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }        
        public string Email { get; set; }
        public CompanyType CompanyType { get; set; }
        public EnumSectors Sector { get; set; }
        public string LogoPath { get; set; }
    }
}
