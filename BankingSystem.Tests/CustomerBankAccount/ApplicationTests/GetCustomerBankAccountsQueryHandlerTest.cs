using BankingSystem.WebApi.CustomerBankAccount.Application.Queries;

namespace BankingSystem.Tests.CustomerBankAccount.ApplicationTests
{
    public class GetCustomerBankAccountsQueryHandlerTest
    {
        private readonly Mock<ICustomerBankAccountRepository> _mockCustomerBankAccountRepository;
        private readonly GetCustomerBankAccountsQueryHandler _sut;

        public GetCustomerBankAccountsQueryHandlerTest()
        {
            _mockCustomerBankAccountRepository = new Mock<ICustomerBankAccountRepository>();
            _sut = new GetCustomerBankAccountsQueryHandler(_mockCustomerBankAccountRepository.Object);
        }

        [Fact]
        public async Task Handle_Calls_GetCustomerWithAllBankAccounts_Once()
        {
            var query = new GetCustomerBankAccountsQuery(1);
            var response = await _sut.Handle(query, default);

            _mockCustomerBankAccountRepository.Verify(x => x.GetCustomerWithAllBankAccounts(1, default), Times.Once);
        }
    }
}