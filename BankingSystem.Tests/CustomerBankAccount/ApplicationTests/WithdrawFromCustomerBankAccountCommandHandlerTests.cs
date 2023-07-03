using AutoFixture;
using BankingSystem.WebApi.Common.Exceptions;
using BankingSystem.WebApi.CustomerBankAccount.Application.Commands;
using BankingSystem.WebApi.CustomerBankAccount.Domain;
using FluentAssertions;

namespace BankingSystem.Tests.CustomerBankAccount.ApplicationTests
{
    public class WithdrawFromCustomerBankAccountCommandHandlerTests
    {
        private readonly Mock<ICustomerBankAccountRepository> _mockCustomerBankAccountRepository;
        private readonly WithdrawFromCustomerBankAccountCommandHandler _sut;
        private readonly Fixture _fixture = new();

        public WithdrawFromCustomerBankAccountCommandHandlerTests()
        {
            _mockCustomerBankAccountRepository = new Mock<ICustomerBankAccountRepository>();
            _sut = new WithdrawFromCustomerBankAccountCommandHandler(_mockCustomerBankAccountRepository.Object);
        }

        [Fact]
        public async Task Handle_WhenCustomerNotFound_ThrowsException()
        {
            //arrange
            var command = new WithdrawFromCustomerBankAccountCommand(1, 1, 100);
            _mockCustomerBankAccountRepository.Setup(o => o.GetCustomerBankAccount(1, 1, It.IsAny<CancellationToken>())).
                    ReturnsAsync(() => null);

            //assert
            var exception = await Assert.ThrowsAsync<DomainException>(() => _sut.Handle(command, default));
            exception.Message.Should().Be($"There is no such customer for the given CustomerId {command.CustomerId}");
            _mockCustomerBankAccountRepository.Verify(x => x.GetCustomerBankAccount(1, 1, default), Times.Once);
            _mockCustomerBankAccountRepository.Verify(x => x.SaveChangesAsync(default), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenBankAccountNotFound_ThrowsException()
        {
            //arrange
            var command = new WithdrawFromCustomerBankAccountCommand(1, 1, 100);
            _mockCustomerBankAccountRepository.Setup(o => o.GetCustomerBankAccount(1, 1, It.IsAny<CancellationToken>())).
                    ReturnsAsync(() => new Customer());

            //assert
            var exception = await Assert.ThrowsAsync<DomainException>(() => _sut.Handle(command, default));
            exception.Message.Should().Be($"There is no such Bank Account with Id {command.BankAccountId} for the customerId {command.CustomerId}");
            _mockCustomerBankAccountRepository.Verify(x => x.GetCustomerBankAccount(1, 1, default), Times.Once);
            _mockCustomerBankAccountRepository.Verify(x => x.SaveChangesAsync(default), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenBankAccountFound_WithdrawAmountGreaterThan90Per_ThrowsException()
        {
            //arrange
            var account = _fixture.Build<BankAccount>()
               .With(b => b.Id, 1)
               .With(b => b.CustomerId, 1)
               .With(b => b.Amount, 100)
               .Create();

            var customer = _fixture.Build<Customer>()
                .With(o => o.Id, 1)
                .With(o => o.BankAccounts, new List<BankAccount>() { account })
                .Create();

            var command = new WithdrawFromCustomerBankAccountCommand(1, 1, 95);
            _mockCustomerBankAccountRepository.Setup(o => o.GetCustomerBankAccount(1, 1, It.IsAny<CancellationToken>())).
                    ReturnsAsync(() => customer);

            var exception = await Assert.ThrowsAsync<DomainException>(() => _sut.Handle(command, default));
            exception.Message.Should().Be($"Cannot withdraw more than 90% of total balance from an account in a single transaction");

            //assert
            _mockCustomerBankAccountRepository.Verify(x => x.GetCustomerBankAccount(1, 1, default), Times.Once);
            _mockCustomerBankAccountRepository.Verify(x => x.SaveChangesAsync(default), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenBankAccountFound_DepositsSuccessfully()
        {
            //arrange
            var account = _fixture.Build<BankAccount>()
               .With(b => b.Id, 1)
               .With(b => b.CustomerId, 1)
               .With(b => b.Amount, 1000)
               .Create();

            var customer = _fixture.Build<Customer>()
                .With(o => o.Id, 1)
                .With(o => o.BankAccounts, new List<BankAccount>() { account })
                .Create();

            var command = new WithdrawFromCustomerBankAccountCommand(1, 1, 100);
            _mockCustomerBankAccountRepository.Setup(o => o.GetCustomerBankAccount(1, 1, It.IsAny<CancellationToken>())).
                    ReturnsAsync(() => customer);

            var result = await _sut.Handle(command, default);

            //assert
            _mockCustomerBankAccountRepository.Verify(x => x.GetCustomerBankAccount(1, 1, default), Times.Once);
            _mockCustomerBankAccountRepository.Verify(x => x.SaveChangesAsync(default), Times.Once);
            result.Should().BeTrue();
        }
    }
}