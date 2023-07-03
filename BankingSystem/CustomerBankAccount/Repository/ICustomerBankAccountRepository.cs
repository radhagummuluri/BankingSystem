namespace BankingSystem.WebApi.CustomerBankAccount.Repository
{
    public interface ICustomerBankAccountRepository
    {
        Task<Domain.Customer?> GetCustomer(string ssn, CancellationToken cancellationToken);
        Task<Domain.Customer?> GetCustomerWithAllBankAccounts(long customerId, CancellationToken cancellationToken);
        Task<Domain.Customer?> GetCustomerBankAccount(long customerId, long bankAccountId, CancellationToken cancellationToken);
        Task<Domain.Customer> SaveCustomer(Domain.Customer customer, bool isNew, CancellationToken cancellationToken);
        Task DeleteBankAccount(Domain.BankAccount account, CancellationToken cancellationToken);
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}