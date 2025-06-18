using MediatR;
using Microsoft.Extensions.Logging;

namespace BackendAuthTemplate.Application.Common.Behaviors
{
    public class UnhandledExceptionBehavior<TRequest, TResponse>(ILogger<TRequest> logger) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken = default)
        {
            try
            {
                return await next(cancellationToken);
            }
            catch (Exception ex)
            {
                string requestName = typeof(TRequest).Name;

                logger.LogError(ex, "CleanArchitecture Request: Unhandled Exception for Request {Name} {@Request}", requestName, request);

                throw;
            }
        }
    }
}
