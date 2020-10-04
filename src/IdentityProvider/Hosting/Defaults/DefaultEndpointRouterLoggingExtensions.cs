using System;
using Microsoft.Extensions.Logging;

namespace IdentityProvider.Hosting.Defaults
{
    public static class DefaultEndpointRouterLoggingExtensions
    {
        private static readonly Action<ILogger, string, string, Exception?> _pathMatchedToHandler;
        private static readonly Action<ILogger, string, Exception?> _noEndpointForPath;
        private static readonly Action<ILogger, string, Exception?> _handlerEnabled;
        private static readonly Action<ILogger, string, Exception?> _handlerDisabled;

        static DefaultEndpointRouterLoggingExtensions()
        {
            _pathMatchedToHandler = LoggerMessage.Define<string, string>(
                LogLevel.Debug,
                new EventId(2_000_000),
                "Request path {path} matched to endpoint handler: {handler}");
            _noEndpointForPath = LoggerMessage.Define<string>(
                LogLevel.Trace,
                new EventId(2_000_001),
                "No endpoint entry found for request path: {path}");
            _handlerEnabled = LoggerMessage.Define<string>(
                LogLevel.Debug,
                new EventId(2_000_002),
                "Handler enabled: {handler}");
            _handlerDisabled = LoggerMessage.Define<string>(
                LogLevel.Warning,
                new EventId(2_000_003),
                "Handler disabled: {handler}");
        }

        public static void PathMatchedToHandler(this ILogger logger, string path, string handler)
        {
            _pathMatchedToHandler(logger, path, handler, null);
        }

        public static void NoEndpointForPath(this ILogger logger, string path)
        {
            _noEndpointForPath(logger, path, null);
        }

        public static void HandlerEnabled(this ILogger logger, string handler)
        {
            _handlerEnabled(logger, handler, null);
        }

        public static void HandlerDisabled(this ILogger logger, string handler)
        {
            _handlerDisabled(logger, handler, null);
        }
    }
}