using PlanetZ.Data.Attributes;
using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Areas.EmployeePage.Models
{
    public class ProfilePageVM
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public DateTime Birthday { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [IsPhoneNumberValid]
        public string PhoneNumber { get; set; }
        public DateTime JobStartDate { get; set; }
        public DateTime? DismissalDate { get; set; }
        public DateTime? NewJobStartDate { get; set; }
        public string PhotoPath { get; set; }
        public string Profession { get; set; }
        public string Title { get; set; }
        public string WorkUnit { get; set; }
        public string Email { get; set; }
        public IFormFile ProfilePhotoFile { get; set; }
    }
}
