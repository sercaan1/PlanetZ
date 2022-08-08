using PlanetZ.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Data
{
    public class Expense
    {
        public int Id { get; set; }

        [Required, MaxLength(250)]
        public string ExpenseDescription { get; set; }

        [Required]
        public decimal ExpenseAmount { get; set; }

        public DateTime RequestDate { get; set; } = DateTime.Now;

        public DateTime? ReplyDate { get; set; }

        public string FilePath { get; set; }

        public EnumExpense ExpenseType { get; set; }

        public EnumExpenseStatus ExpenseStatus { get; set; }

        public string ReturnResponse { get; set; }

        public string EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
