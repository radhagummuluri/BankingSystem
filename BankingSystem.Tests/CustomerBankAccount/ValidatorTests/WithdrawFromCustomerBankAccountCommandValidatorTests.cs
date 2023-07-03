using BankingSystem.WebApi.CustomerBankAccount.Application.Commands;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace BankingSystem.Tests.CustomerBankAccount.ValidatorTests
{
    public class WithdrawFromCustomerBankAccountCommandValidatorTests
    {
        private WithdrawFromCustomerBankAccountCommand _command = null!;
        private WithdrawFromCustomerBankAccountCommandValidator _validator = null!;

        public WithdrawFromCustomerBankAccountCommandValidatorTests()
        {
            _validator = new WithdrawFromCustomerBankAccountCommandValidator();
        }

        [Fact]
        public void WithdrawFromCustomerBankAccountCommand_CustomerIdNotSpecified_ThrowsException()
        {
            _command = new WithdrawFromCustomerBankAccountCommand(0, 1, 200);
            var result = _validator.TestValidate(_command);
            result.ShouldHaveValidationErrorFor(i => i.CustomerId).WithErrorMessage("CustomerId value is invalid");
        }

        [Fact]
        public void WithdrawFromCustomerBankAccountCommand_BankAccountIdNotSpecified_ThrowsException()
        {
            _command = new WithdrawFromCustomerBankAccountCommand(1, 0, 200);
            var result = _validator.TestValidate(_command);
            result.ShouldHaveValidationErrorFor(i => i.BankAccountId).WithErrorMessage("BankAccountId value is invalid");
        }

        [Fact]
        public void WithdrawFromCustomerBankAccountCommand_DepositAmountIsZero_ThrowsException()
        {
            _command = new WithdrawFromCustomerBankAccountCommand(1, 1, 0);
            var result = _validator.TestValidate(_command);
            result.ShouldHaveValidationErrorFor(i => i.Amount).WithErrorMessage("Amount should be greater than $0");
        }

        [Fact]
        public void WithdrawFromCustomerBankAccountCommand_Valid_Success()
        {
            _command = new WithdrawFromCustomerBankAccountCommand(1, 1, 1000);
            var result = _validator.TestValidate(_command);
            result.IsValid.Should().BeTrue();
        }
    }
}