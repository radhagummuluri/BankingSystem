using BankingSystem.Data;
using BankingSystem.WebApi.CustomerBankAccount.Domain;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.WebApi.CustomerBankAccount.Repository
{
    public class CustomerBankAccountRepository : ICustomerBankAccountRepository
    {
        private readonly BankingContext _dbContext;

        public CustomerBankAccountRepository(BankingContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Customer?> GetCustomer(string ssn, CancellationToken cancellationToken) =>
            await _dbContext.Customers.FirstOrDefaultAsync(c => c.Ssn == ssn, cancellationToken);

        public async Task<Customer?> GetCustomerWithAllBankAccounts(long customerId, CancellationToken cancellationToken) =>
            await _dbContext.Customers
            .Include(c => c.BankAccounts)
            .FirstOrDefaultAsync(c => c.Id == customerId, cancellationToken);

        public async Task<Customer?> GetCustomerBankAccount(long customerId, long bankAccountId, CancellationToken cancellationToken) =>
            await _dbContext.Customers
            .Include(c => c.BankAccounts.Where(ba => ba.Id == bankAccountId))
            .FirstOrDefaultAsync(x => x.Id == customerId, cancellationToken);

        public async Task<Customer> SaveCustomer(Customer customer, bool isNew, CancellationToken cancellationToken)
        {
            if(customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            if (isNew)
            {
                _dbContext.Add(customer);
            }
            await _dbContext.SaveChangesAsync();   
            return customer;
        }

        public async Task DeleteBankAccount(BankAccount account, CancellationToken cancellationToken)
        {
            if(account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }
            _dbContext.BankAccounts.Remove(account);
            await _dbContext.SaveChangesAsync();
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
