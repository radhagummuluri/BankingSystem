using AutoFixture;
using BankingSystem.WebApi.CustomerBankAccount.Application.Commands;
using BankingSystem.WebApi.CustomerBankAccount.Domain;
using FluentAssertions;

namespace BankingSystem.Tests.CustomerBankAccount.ApplicationTests
{
    public class CreateCustomerBankAccountCommandHandlerTests
    {
        private readonly Mock<ICustomerBankAccountRepository> _mockCustomerBankAccountRepository;
        private readonly CreateCustomerBankAccountCommandHandler _sut;
        private readonly Fixture _fixture = new();

        public CreateCustomerBankAccountCommandHandlerTests()
        {
            _mockCustomerBankAccountRepository = new Mock<ICustomerBankAccountRepository>();
            _sut = new CreateCustomerBankAccountCommandHandler(_mockCustomerBankAccountRepository.Object);
        }

        [Fact]
        public async Task Handle_WhenNewCustomer_CreatesCustomerWithBankAccount()
        {
            //arrange
            var account = _fixture.Build<BankAccount>()
                .With(b => b.CustomerId, 1)
                .Create();
            
            var customer = _fixture.Build<Customer>()
                .With(o => o.Id, 1)
                .With(o => o.BankAccounts, new List<BankAccount>() { account })
                .Create();

            var command = new CreateCustomerBankAccountCommand(customer.Ssn, customer.FirstName, customer.LastName, account.Amount);
            _mockCustomerBankAccountRepository.Setup(o => o.GetCustomer(customer.Ssn, It.IsAny<CancellationToken>())).
                    ReturnsAsync(() => null);
            _mockCustomerBankAccountRepository.Setup(o => o.SaveCustomer(It.IsAny<Customer>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).
                    ReturnsAsync(customer);

            //act
            var response = await _sut.Handle(command, default);

            //assert
            _mockCustomerBankAccountRepository.Verify(x => x.GetCustomer(customer.Ssn, default), Times.Once);
            _mockCustomerBankAccountRepository.Verify(x => x.SaveCustomer(It.IsAny<Customer>(), true, default), Times.Once);
            response.Should().NotBeNull();
            response.CustomerId.Should().Be(customer.Id);
            response.BankAccountId.Should().Be(account.Id);
        }

        [Fact]
        public async Task Handle_WhenExistingCustomer_CreatesNewBankAccount()
        {
            //arrange
            var existingCustomer = _fixture.Build<Customer>()
                .With(o => o.Id, 1)
                .With(o => o.BankAccounts, new List<BankAccount>())
                .Create();

            var account = _fixture.Build<BankAccount>()
                .With(b => b.CustomerId, 1)
                .Create();
            
            var updatedCustomer = new Customer { 
                Id= existingCustomer.Id,
                FirstName = existingCustomer.FirstName, 
                LastName = existingCustomer.LastName,
                Ssn = existingCustomer.Ssn,
                BankAccounts = new List<BankAccount>() { account }
            };

            var command = new CreateCustomerBankAccountCommand(existingCustomer.Ssn, existingCustomer.FirstName, existingCustomer.LastName, account.Amount);
            _mockCustomerBankAccountRepository.Setup(o => o.GetCustomer(existingCustomer.Ssn, It.IsAny<CancellationToken>())).
                    ReturnsAsync(existingCustomer);

            _mockCustomerBankAccountRepository.Setup(o => o.SaveCustomer(It.IsAny<Customer>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).
                    ReturnsAsync(updatedCustomer);

            //act
            var response = await _sut.Handle(command, default);

            //assert
            _mockCustomerBankAccountRepository.Verify(x => x.GetCustomer(existingCustomer.Ssn, default), Times.Once);
            _mockCustomerBankAccountRepository.Verify(x => x.SaveCustomer(It.IsAny<Customer>(), false, default), Times.Once);
            response.Should().NotBeNull();
            response.CustomerId.Should().Be(existingCustomer.Id);
            response.BankAccountId.Should().Be(account.Id);
        }
    }
}