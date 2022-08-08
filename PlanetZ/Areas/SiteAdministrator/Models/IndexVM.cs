using PlanetZ.Data;
using PlanetZ.Data.Enums;

namespace PlanetZ.Areas.SiteAdministrator.Models
{
    public class IndexVM
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string PhotoPath { get; set; }
        public string WorkUnit { get; set; }
        public string Profession { get; set; }
        public Company Company { get; set; }
        public EnumRoles Role { get; set; }
        public EnumActivityStatus ActivityStatus { get; set; } = EnumActivityStatus.Active;
        public List<ListCompanyVM> Companies { get; set; }
        public List<PackagesCardVM> Packages { get; set; }


    }
}
