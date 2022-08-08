using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Areas.CompanyManager.Models.ViewModels
{
    public class TransferBalanceVM
    {
        public int WalletId { get; set; }

        public List<SelectListItem> CreditCards { get; set; }

        public int CreditCardId { get; set; }

        public decimal WalletBalance { get; set; }

        [Required]
        [Range(1, 2001, ErrorMessage = "You can transfer min amount of ₺1, max ₺2000")]
        public decimal TransferAmount { get; set; }
    }
}
