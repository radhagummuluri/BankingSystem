using BankingSystem.WebApi.CustomerBankAccount.Repository;

namespace Microsoft.Extensions.DependencyInjection {
   public static class CustomerBankAccountServiceCollectionExtensions {
      public static IServiceCollection AddCustomerBankAccountServices(this IServiceCollection services) =>
         services
            .AddScoped<ICustomerBankAccountRepository, CustomerBankAccountRepository>();
   }
}
