namespace BankingSystem.WebApi.Common.Exceptions
{
    public class DomainException : Exception
    {
        public string[] ErrorMessages { get; set; }
        public DomainException(params string[] errorMessages) : base(string.Join(", ", errorMessages))
        {
            ErrorMessages = errorMessages;
        }

        public DomainException(string msg, Exception inner) : base(msg, inner)
        {
            ErrorMessages = Array.Empty<string>();
        }
    }
}
