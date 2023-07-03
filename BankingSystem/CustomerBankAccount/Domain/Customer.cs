namespace BankingSystem.WebApi.CustomerBankAccount.Domain
{
    public class Customer
    {
        public long Id { get; set; }
        public string Ssn { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public ICollection<BankAccount> BankAccounts {get; set;} = new List<BankAccount>();
    }
}
