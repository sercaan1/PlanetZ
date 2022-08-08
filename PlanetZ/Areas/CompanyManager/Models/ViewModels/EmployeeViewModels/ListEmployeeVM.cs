using PlanetZ.Data.Enums;

namespace PlanetZ.Areas.CompanyManager.Models.ViewModels.EmployeeViewModels
{
    public class ListEmployeeVM
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhotoPath { get; set; }
        public string Title { get; set; }
        public EnumActivityStatus ActivityStatus { get; set; }
    }
}
