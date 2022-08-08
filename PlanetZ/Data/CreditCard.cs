using PlanetZ.Data.Attributes;
using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Data
{
    public class CreditCard
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        [OnlyLetter]
        public string Name { get; set; }

        [Required]
        public decimal Balance { get; set; }

        [CreditCard(ErrorMessage = "Invalid card number")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^(0[1-9]|1[1-2])\/[0-9]{2}$", ErrorMessage = "Invalid card expire date")]
        public string CardExpire { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression("^[0-9]{3,4}$", ErrorMessage = "Invalid cvv number")]
        public string CardCvv { get; set; }

        public int WalletId { get; set; }

        public Wallet Wallet { get; set; }
    }
}
