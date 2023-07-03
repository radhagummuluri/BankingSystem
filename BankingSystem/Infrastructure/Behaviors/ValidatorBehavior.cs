using FluentValidation;
using MediatR;

namespace BankingSystem.WebApi.Infrastructure.Behaviors
{
    public class ValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
       where TRequest : IRequest<TResponse>
    {
        private readonly IValidator<TRequest>? _validator;

        public ValidatorBehavior(IValidator<TRequest> validator)
        {
            _validator = validator;
        }

        public ValidatorBehavior() { }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validator != null)
            {
                await ValidateRequest(_validator, request);
            }
            return await next();
        }

        private static async Task ValidateRequest(IValidator<TRequest> validator, TRequest request)
        {
            var result = await validator.ValidateAsync(request);

            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
        }
    }
}
