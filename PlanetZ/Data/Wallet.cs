using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Data
{
    public class Wallet
    {
        public int Id { get; set; }

        [Required]
        public decimal Balance { get; set; } = 0.00m;

        public int CompanyId { get; set; }

        public Company Company { get; set; }

        public List<CreditCard> CreditCards { get; set; }
    }
}
