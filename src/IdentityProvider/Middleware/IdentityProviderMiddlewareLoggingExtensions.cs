using System;
using Microsoft.Extensions.Logging;

namespace IdentityProvider.Middleware
{
    public static class IdentityProviderMiddlewareLoggingExtensions
    {
        private static readonly Action<ILogger, string, string, Exception?> _invokingHandler;
        private static readonly Action<ILogger, string, Exception?> _applyingResult;

        static IdentityProviderMiddlewareLoggingExtensions()
        {
            _invokingHandler = LoggerMessage.Define<string, string>(
                LogLevel.Information,
                new EventId(1_000_000),
                "Invoking IdentityProvider handler: {handler} for {uri}");
            _applyingResult = LoggerMessage.Define<string>(
                LogLevel.Trace,
                new EventId(1_000_001),
                "Applying result: {result}");
        }

        public static void InvokingHandler(this ILogger logger, string handler, string uri)
        {
            _invokingHandler(logger, handler, uri, null);
        }

        public static void ApplyingResult(this ILogger logger, string result)
        {
            _applyingResult(logger, result, null);
        }
    }
}