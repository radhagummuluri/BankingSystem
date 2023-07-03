using AutoFixture;
using BankingSystem.WebApi.Common.Exceptions;
using BankingSystem.WebApi.CustomerBankAccount.Application.Commands;
using BankingSystem.WebApi.CustomerBankAccount.Domain;
using FluentAssertions;

namespace BankingSystem.Tests.CustomerBankAccount.ApplicationTests
{
    public class DeleteCustomerBankAccountCommandHandlerTests
    {
        private readonly Mock<ICustomerBankAccountRepository> _mockCustomerBankAccountRepository;
        private readonly DeleteCustomerBankAccountCommandHandler _sut;
        private readonly Fixture _fixture = new();

        public DeleteCustomerBankAccountCommandHandlerTests()
        {
            _mockCustomerBankAccountRepository = new Mock<ICustomerBankAccountRepository>();
            _sut = new DeleteCustomerBankAccountCommandHandler(_mockCustomerBankAccountRepository.Object);
        }

        [Fact]
        public async Task Handle_WhenCustomerNotFound_ThrowsException()
        {
            //arrange
            var command = new DeleteCustomerBankAccountCommand(1, 1);
            _mockCustomerBankAccountRepository.Setup(o => o.GetCustomerBankAccount(1, 1, It.IsAny<CancellationToken>())).
                    ReturnsAsync(() => null);

            //assert
            var exception = await Assert.ThrowsAsync<DomainException>(() => _sut.Handle(command, default));
            exception.Message.Should().Be($"There is no such customer for the given CustomerId {command.CustomerId}");
            _mockCustomerBankAccountRepository.Verify(x => x.GetCustomerBankAccount(1, 1, default), Times.Once);
            _mockCustomerBankAccountRepository.Verify(x => x.DeleteBankAccount(It.IsAny<BankAccount>() , default), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenBankAccountNotFound_ThrowsException()
        {
            //arrange
            var command = new DeleteCustomerBankAccountCommand(1, 1);
            _mockCustomerBankAccountRepository.Setup(o => o.GetCustomerBankAccount(1, 1, It.IsAny<CancellationToken>())).
                    ReturnsAsync(() => new Customer());

            //assert
            var exception = await Assert.ThrowsAsync<DomainException>(() => _sut.Handle(command, default));
            exception.Message.Should().Be($"There is no such Bank Account with Id {command.BankAccountId} for the customerId {command.CustomerId}");
            _mockCustomerBankAccountRepository.Verify(x => x.GetCustomerBankAccount(1, 1, default), Times.Once);
            _mockCustomerBankAccountRepository.Verify(x => x.DeleteBankAccount(It.IsAny<BankAccount>(), default), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenBankAccountFound_DeleteBankAccountSuccessfully()
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

            var command = new DeleteCustomerBankAccountCommand(1, 1);
            _mockCustomerBankAccountRepository.Setup(o => o.GetCustomerBankAccount(1, 1, It.IsAny<CancellationToken>())).
                    ReturnsAsync(() => customer);

            var result = await _sut.Handle(command, default);

            //assert
            _mockCustomerBankAccountRepository.Verify(x => x.GetCustomerBankAccount(1, 1, default), Times.Once);
            _mockCustomerBankAccountRepository.Verify(x => x.DeleteBankAccount(It.IsAny<BankAccount>(), default), Times.Once);
            result.Should().BeTrue();
        }
    }
}