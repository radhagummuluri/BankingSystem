using BankingSystem.WebApi.CustomerBankAccount.Application.Queries;

namespace BankingSystem.Tests.CustomerBankAccount.ApplicationTests
{
    public class GetCustomerBankAccountQueryHandlerTest
    {
        private readonly Mock<ICustomerBankAccountRepository> _mockCustomerBankAccountRepository;
        private readonly GetCustomerBankAccountQueryHandler _sut;

        public GetCustomerBankAccountQueryHandlerTest()
        {
            _mockCustomerBankAccountRepository = new Mock<ICustomerBankAccountRepository>();
            _sut = new GetCustomerBankAccountQueryHandler(_mockCustomerBankAccountRepository.Object);
        }

        [Fact]
        public async Task Handle_Calls_GetCustomerBankAccount_Once()
        {
            var query = new GetCustomerBankAccountQuery(1, 1);
            var response = await _sut.Handle(query, default);

            _mockCustomerBankAccountRepository.Verify(x => x.GetCustomerBankAccount(1, 1, default), Times.Once);
        }
    }
}