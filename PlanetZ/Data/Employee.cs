using Microsoft.AspNetCore.Identity;
using PlanetZ.Data.Attributes;
using PlanetZ.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Data
{
    public class Employee : IdentityUser
    {
        [Required, MaxLength(50)]
        [OnlyLetter]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        [OnlyLetter]
        public string LastName { get; set; }

        [AgeControl]
        [Required]
        public DateTime Birthday { get; set; }

        [Required]
        [StartDateControl]
        public DateTime JobStartDate { get; set; }

        [StartDateControl]
        public DateTime? NewJobStartDate { get; set; }

        [Required]
        public string Address { get; set; }

        [Required, MaxLength(11)]
        public string Identity { get; set; }

        [Required]
        public EnumRoles Role { get; set; }

        [Required]
        public EnumBloodGroup BloodGroup { get; set; }

        [OnlyLetter]
        [Required]
        public string Profession { get; set; }

        [OnlyLetter]
        [Required]
        public string Title { get; set; }

        [OnlyLetter]
        [Required]
        public string WorkUnit { get; set; }

        [Required]
        public decimal Salary { get; set; }

        public EnumGender Gender { get; set; }
        public DateTime? DismissalDate { get; set; }
        public string PhotoPath { get; set; }
        public EnumMaritalStatus MaritalStatus { get; set; } = EnumMaritalStatus.Unknown;
        public EnumActivityStatus ActivityStatus
        {
            get
            {
                if (JobStartDate < DateTime.Now && DismissalDate == null)
                    return EnumActivityStatus.Active;
                else if (NewJobStartDate < DateTime.Now && DismissalDate < NewJobStartDate)
                    return EnumActivityStatus.Active;
                else
                    return EnumActivityStatus.Passive;
            }
            set
            {
            }
        }
        public bool DidPasswordChange { get; set; } = false;

        public int AnnualLeave { get; set; }

        public Company Company { get; set; }

        public int CompanyId { get; set; }

        public List<Permission> Permissions { get; set; }

        public List<Advance> Advances { get; set; }

        public List<Expense> Expenses { get; set; }
    }
}
