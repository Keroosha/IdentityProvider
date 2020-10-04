using System;
using Microsoft.Extensions.Logging;

namespace IdentityProvider.Middleware
{
    public static class IdentityProviderMiddlewareLoggingExtensions
    {
        private static readonly Action<ILogger, string, string, Exception?> _invokingEndpoint;
        private static readonly Action<ILogger, string, Exception?> _executingResult;

        static IdentityProviderMiddlewareLoggingExtensions()
        {
            _invokingEndpoint = LoggerMessage.Define<string, string>(
                LogLevel.Information,
                new EventId(1_000_000),
                "Invoking IdentityProvider endpoint: {endpoint} for {uri}");
            _executingResult = LoggerMessage.Define<string>(
                LogLevel.Trace,
                new EventId(1_000_001),
                "Invoking result: {result}");
        }

        public static void InvokingEndpoint(this ILogger logger, string handler, string uri)
        {
            _invokingEndpoint(logger, handler, uri, null);
        }

        public static void ExecutingResult(this ILogger logger, string result)
        {
            _executingResult(logger, result, null);
        }
    }
}