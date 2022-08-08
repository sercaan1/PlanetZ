using PlanetZ.Data.Attributes;
using PlanetZ.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Areas.SiteAdministrator.Models
{
    public class CreatePackageVM
    {
        [Required(ErrorMessage = "This area is required")]
        [MaxLength(50)]
        public string PackageName { get; set; }
       
        [Required(ErrorMessage = "This area is required")]    
        public int? Amount { get; set; }

        [Required(ErrorMessage = "This area is required")]
        [EndOfContract]
        public DateTime? PublishStartDay { get; set; }

        [Required(ErrorMessage = "This area is required")]
        [PublishEndDate]
        public DateTime? PublishEndDay { get; set; }

        [Required(ErrorMessage = "This area is required")]
        [Display(Name ="Usage Time")]
        //gün sayısı olarak girileceğini varsaydığım için int türünde oluşturuyorum.
        public int? UsageTime { get; set; }

        [Required(ErrorMessage = "This area is required")]
        [NumberOfUsers]
        public int? NumberofUsers { get; set; }
        public IFormFile PhotoFile { get; set; }
        public EnumCurrencyUnit CurrencyUnit { get; set; }

        [Required(ErrorMessage = "This area is required")]
        [MaxLength(250)]
        public string Description { get; set; }
    }
}
