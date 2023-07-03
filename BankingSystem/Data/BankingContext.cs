using BankingSystem.WebApi.CustomerBankAccount.Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BankingSystem.Data
{
    public class BankingContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public BankingContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseInMemoryDatabase("TestDb");
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
               .Entity<Customer>()
               .HasMany(c => c.BankAccounts)
               .WithOne()
               .HasForeignKey("CustomerId")
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
