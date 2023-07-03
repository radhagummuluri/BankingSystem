using BankingSystem.WebApi.Infrastructure.Behaviors;
using MediatR;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class BehaviorsServiceCollectionExtensions
    {
        public static IServiceCollection AddBehaviors(this IServiceCollection services) =>
           services
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
    }
}
