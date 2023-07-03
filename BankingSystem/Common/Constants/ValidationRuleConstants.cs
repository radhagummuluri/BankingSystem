namespace BankingSystem.WebApi.Common.Constants
{
    public static class ValidationRuleConstants
    {
        public const string SSN_REGEX = "^(?!666|000|9\\d{2})\\d{3}-(?!00)\\d{2}-(?!0{4})\\d{4}$";
        public const string SSN_VALIDATION_MESSAGE = "SSN should be specified in XXX-XX-XXXX format";
    }
}
