using MediatR;
using System.Reflection;

namespace BankingSystem.WebApi.Infrastructure.Behaviors
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

        public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            //Request
            _logger.LogInformation($"Starting request {typeof(TRequest).Name}, {DateTime.UtcNow}");
            Type myType = request.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());

            foreach (PropertyInfo prop in props)
            {
                object propValue = prop.GetValue(request, null);
                _logger.LogInformation("{Property} : {@Value}", prop.Name, propValue);
            }
            TResponse result;
            try
            {
                result = await next();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Request failure {typeof(TRequest).Name}, {ex.Message} , {DateTime.UtcNow}");
                throw;
            }

            //Response
            _logger.LogInformation($"Handled {typeof(TResponse).Name}, {DateTime.UtcNow}");
            return result;
        }
    }
}
