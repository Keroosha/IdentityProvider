using System;
using Microsoft.Extensions.Logging;

namespace IdentityProvider.Hosting.Defaults
{
    public static class DefaultEndpointRouterLoggingExtensions
    {
        private static readonly Action<ILogger, string, string, Exception?> _pathMatchedToEndpoint;
        private static readonly Action<ILogger, string, Exception?> _noEndpointForPath;
        private static readonly Action<ILogger, string, Exception?> _endpointEnabled;
        private static readonly Action<ILogger, string, Exception?> _endpointDisabled;

        static DefaultEndpointRouterLoggingExtensions()
        {
            _pathMatchedToEndpoint = LoggerMessage.Define<string, string>(
                LogLevel.Debug,
                new EventId(2_000_000),
                "Request path {path} matched to endpoint: {endpoint}");
            _noEndpointForPath = LoggerMessage.Define<string>(
                LogLevel.Trace,
                new EventId(2_000_001),
                "No endpoint entry found for request path: {path}");
            _endpointEnabled = LoggerMessage.Define<string>(
                LogLevel.Debug,
                new EventId(2_000_002),
                "Endpoint enabled: {endpoint}");
            _endpointDisabled = LoggerMessage.Define<string>(
                LogLevel.Warning,
                new EventId(2_000_003),
                "Endpoint disabled: {endpoint}");
        }

        public static void PathMatchedToEndpoint(this ILogger logger, string path, string handler)
        {
            _pathMatchedToEndpoint(logger, path, handler, null);
        }

        public static void NoEndpointForPath(this ILogger logger, string path)
        {
            _noEndpointForPath(logger, path, null);
        }

        public static void EndpointEnabled(this ILogger logger, string handler)
        {
            _endpointEnabled(logger, handler, null);
        }

        public static void EndpointDisabled(this ILogger logger, string handler)
        {
            _endpointDisabled(logger, handler, null);
        }
    }
}