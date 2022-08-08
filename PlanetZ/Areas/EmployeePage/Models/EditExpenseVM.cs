using PlanetZ.Data.Attributes;
using PlanetZ.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Areas.EmployeePage.Models
{
    public class EditExpenseVM
    {
        public int Id { get; set; }

        public string FilePath { get; set; }

        [Required(ErrorMessage = "The Amount field is required")]
        [ExpenseMinAndMaxControl]
        public decimal ExpenseAmount { get; set; }

        public EnumExpense ExpenseType { get; set; }

        [IsFileValid(MaxFileSize = 1024)]
        public IFormFile File { get; set; }
    }
}
