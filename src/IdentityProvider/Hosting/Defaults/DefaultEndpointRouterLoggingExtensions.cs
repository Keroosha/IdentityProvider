using System;
using Microsoft.Extensions.Logging;

namespace IdentityProvider.Hosting.Defaults
{
    public static class DefaultEndpointRouterLoggingExtensions
    {
        private static readonly Action<ILogger, string, string, Exception?> _pathMatchedToHandler;
        private static readonly Action<ILogger, string, Exception?> _noEndpointForPath;

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
        }

        public static void PathMatchedToHandler(this ILogger logger, string path, string handler)
        {
            _pathMatchedToHandler(logger, path, handler, null);
        }

        public static void NoEndpointForPath(this ILogger logger, string path)
        {
            _noEndpointForPath(logger, path, null);
        }
    }
}