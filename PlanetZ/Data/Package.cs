using PlanetZ.Data.Attributes;
using PlanetZ.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Data
{
    public class Package
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="This area is required")]
        [MaxLength(50)]       
        public string PackageName { get; set; }
        [Required(ErrorMessage = "This area is required")]
        public int Amount { get; set; }
        [Required(ErrorMessage = "This area is required")]
        //gün sayısı olarak girileceğini varsaydığım için int türünde oluşturuyorum.
        public int UsageTime
        {
            get
            {
                return (int)((TimeSpan)(PublishEndDay - PublishStartDay)).TotalDays;
            }
            set
            {
            }
        }
        [Required(ErrorMessage = "This area is required")]        
        [NumberOfUsers]
        public int NumberofUsers { get; set; }
        [Required(ErrorMessage = "This area is required")]
        [PublishStartDay]
        public DateTime PublishStartDay { get; set; }
        [Required]
        [PublishEndDate]
        public DateTime PublishEndDay { get; set; }
        [Required(ErrorMessage = "This area is required")]
        public DateTime CreationDate { get; set; }
        public EnumActivityStatus ActivityStatus
        {
            get
            {
                if (PublishStartDay < DateTime.Now && PublishEndDay > DateTime.Now)
                {
                    return EnumActivityStatus.Active;
                }
                else
                {
                    return EnumActivityStatus.Passive;
                }
            }
            set
            {

            }
        }
        public string PhotoPath { get; set; }
        public EnumCurrencyUnit CurrencyUnit { get; set; }
        [Required(ErrorMessage = "This area is required")]
        [MaxLength(250)]
        public string Description { get; set; }
        public List<Company> Companies { get; set; }
    }
}

