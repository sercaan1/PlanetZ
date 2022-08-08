using PlanetZ.Data.Attributes;
using PlanetZ.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlanetZ.Areas.EmployeePage.Models
{
    public class CreateExpenseVM
    {
        [Required(ErrorMessage = "The Description field is required"), MaxLength(250)]
        public string ExpenseDescription { get; set; }

        [Required(ErrorMessage = "The Amount field is required")]
        [ExpenseMinAndMaxControl]
        public decimal ExpenseAmount { get; set; }

        public DateTime RequestDate { get; set; } = DateTime.Now;

        [NotMapped]
        [IsFileValid(MaxFileSize = 1024)]
        public IFormFile FilePath { get; set; }

        public EnumExpense ExpenseType { get; set; }
    }
}
