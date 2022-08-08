using PlanetZ.Data.Enums;

namespace PlanetZ.Areas.SiteAdministrator.Models
{
    public class DetailsPackageVM
    {
        public int Id { get; set; }
        
        public string PackageName { get; set; }

        public int Amount { get; set; }

        public int UsageTime { get; set; }

        public int NumberofUsers { get; set; }

        public DateTime PublishStartDay { get; set; }

        public DateTime PublishEndDay { get; set; }

        public EnumActivityStatus ActivityStatus { get; set; }

        public string PhotoPath { get; set; }
    }
}
