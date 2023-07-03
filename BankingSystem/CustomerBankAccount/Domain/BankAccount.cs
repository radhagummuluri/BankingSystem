namespace BankingSystem.WebApi.CustomerBankAccount.Domain
{
    public class BankAccount
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public decimal Amount { get; set; }
    }
}