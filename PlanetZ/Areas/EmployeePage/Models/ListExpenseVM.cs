using PlanetZ.Data.Enums;

namespace PlanetZ.Areas.EmployeePage.Models
{
    public class ListExpenseVM
    {
        public int Id { get; set; }
        public string EmployeeFullName { get; set; }

        public string ExpenseDescription { get; set; }
        
        public decimal ExpenseAmount { get; set; }

        public DateTime RequestDate { get; set; } 

        public DateTime? ReplyDate { get; set; }

        public string FilePath { get; set; }

        public EnumExpense ExpenseType { get; set; }

        public EnumExpenseStatus ExpenseStatus { get; set; }

        public string ReturnResponse { get; set; }
    }
}
