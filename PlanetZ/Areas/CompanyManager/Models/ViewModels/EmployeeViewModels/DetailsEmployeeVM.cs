using PlanetZ.Data.Attributes;
using PlanetZ.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Areas.CompanyManager.Models.ViewModels.EmployeeViewModels
{
    public class DetailsEmployeeVM
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        //
        public DateTime JobStartDate { get; set; }
        public DateTime? NewJobStartDate { get; set; } 
        public DateTime? DismissalDate { get; set; } 
        //
        public string Email { get; set; }
        public string Address { get; set; }
        public string Identity { get; set; }

        [IsPhoneNumberValid]
        public string PhoneNumber { get; set; }
        public EnumRoles Role { get; set; }
        public string Profession { get; set; }
        public string Title { get; set; }
        public string WorkUnit { get; set; }        
        public EnumBloodGroup BloodGroup { get; set; }
        public EnumGender Gender { get; set; }
        public string PhotoPath { get; set; }
        public EnumMaritalStatus MaritalStatus { get; set; }
        public EnumActivityStatus ActivityStatus { get; set; }
        public IFormFile ProfilePhotoFile { get; set; }
    }
}
