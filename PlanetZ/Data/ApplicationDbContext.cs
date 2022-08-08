using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PlanetZ.Data
{
    public class ApplicationDbContext : IdentityDbContext<Employee>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<Company> Companies { get; set; }

        public DbSet<Permission> Permissions { get; set; }

        public DbSet<Advance> Advances { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Wallet>().HasOne(x => x.Company)
                .WithOne(x => x.Wallet).HasForeignKey<Wallet>(x => x.CompanyId);

            builder.Entity<Wallet>().HasMany(x => x.CreditCards)
                .WithOne(x => x.Wallet);
        }
    }
}