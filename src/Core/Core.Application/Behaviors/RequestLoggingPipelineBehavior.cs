using Core.Shared.Results;
using MediatR;
using Serilog;
using Serilog.Context;

namespace Core.Application.Behaviors
{
    public sealed class RequestLoggingPipelineBehavior<TRequest, TResponse>(
        ILogger logger) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class
        where TResponse : Result
    {
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            string moduleName = GetModuleName(typeof(TRequest).FullName!);
            string requestName = typeof(TRequest).Name;

            using (LogContext.PushProperty("Module", moduleName))
            {
                logger.Information("Processing request {RequestName}", requestName);

                TResponse result = await next();

                if (result.IsSuccess)
                {
                    logger.Information("Completed request {RequestName}", requestName);
                }
                else
                {
                    using (LogContext.PushProperty("Error", result.Error, true))
                    {
                        logger.Error("Completed request {RequestName} with error", requestName);
                    }
                }

                return result;
            }
        }

        private static string GetModuleName(string requestName) => requestName.Split('.')[0];
    }}
