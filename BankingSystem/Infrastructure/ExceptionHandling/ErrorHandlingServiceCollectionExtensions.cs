using BankingSystem.WebApi.Common;
using BankingSystem.WebApi.Common.Exceptions;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ErrorHandlingServiceCollectionExtensions
    {
        private const string EXCEPTION_DETAILS_PROPERTY = "exception";
        private const string ERRORS_PROPERTY = "errors";
        public static IServiceCollection AddExceptionHandling(this IServiceCollection services)
        {
            return services.AddProblemDetails(options => {
                options.IncludeExceptionDetails = (ctx, ex) => false;
                options.ExceptionDetailsPropertyName = EXCEPTION_DETAILS_PROPERTY;

                options.Map<DomainException>(ex => {
                    var details = new ProblemDetails
                    {
                        Title = "One or more domain errors occurred.",
                        Status = StatusCodes.Status400BadRequest
                    };
                    details.Extensions[ERRORS_PROPERTY] = ex.Message;

                    return details;
                });

                options.Map<ValidationException>(ex => {
                    var validationErrors = ex.Errors
                          .GroupBy(e => e.PropertyName)
                          .ToDictionary(e => e.Key, e => e.Select(p => p.ErrorMessage).ToArray());

                    return new ValidationProblemDetails(validationErrors)
                    {
                        Title = "One or more validation errors occurred.",
                        Status = StatusCodes.Status400BadRequest
                    };
                });
            });
        }
    }
}