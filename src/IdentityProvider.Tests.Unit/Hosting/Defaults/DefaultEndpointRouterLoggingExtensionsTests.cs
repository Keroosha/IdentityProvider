using IdentityProvider.Hosting.Defaults;
using IdentityProvider.Tests.Unit.Common.Logging;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace IdentityProvider.Tests.Unit.Hosting.Defaults
{
    public class DefaultEndpointRouterLoggingExtensionsTests
    {
        [TestCase("/foo", "FooEndpoint")]
        [TestCase("/bar", "BarEndpoint")]
        public void PathMatchedToEndpoint_should_log_debug_with_eventId_2_000_000(string path, string endpoint)
        {
            var logger = new FakeLogger();
            var expectedLoggedValue = $"Request path {path} matched to endpoint: {endpoint}";

            LoggingAssertions.CallExtensionsAndAssertLog(
                logger,
                DefaultEndpointRouterLoggingExtensions.PathMatchedToEndpoint,
                path,
                endpoint,
                LogLevel.Debug,
                new EventId(2_000_000),
                expectedLoggedValue);
        }

        [TestCase("/foo")]
        [TestCase("/bar")]
        public void NoEndpointForPath_should_log_trace_with_eventId_2_000_001(string path)
        {
            var logger = new FakeLogger();
            var expectedLoggedValue = $"No endpoint entry found for request path: {path}";

            LoggingAssertions.CallExtensionsAndAssertLog(
                logger,
                DefaultEndpointRouterLoggingExtensions.NoEndpointForPath,
                path,
                LogLevel.Trace,
                new EventId(2_000_001),
                expectedLoggedValue);
        }

        [TestCase(Constants.EndpointNames.Authorize)]
        [TestCase(Constants.EndpointNames.Discovery)]
        [TestCase("CustomEndpoint")]
        public void EndpointEnabled_should_log_debug_with_eventId_2_000_002(string endpoint)
        {
            var logger = new FakeLogger();
            var expectedLoggedValue = $"Endpoint enabled: {endpoint}";

            LoggingAssertions.CallExtensionsAndAssertLog(
                logger,
                DefaultEndpointRouterLoggingExtensions.EndpointEnabled,
                endpoint,
                LogLevel.Debug,
                new EventId(2_000_002),
                expectedLoggedValue);
        }

        [TestCase(Constants.EndpointNames.Authorize)]
        [TestCase(Constants.EndpointNames.Discovery)]
        [TestCase("CustomEndpoint")]
        public void EndpointDisabled_should_log_warning_with_eventId_2_000_003(string endpoint)
        {
            var logger = new FakeLogger();
            var expectedLoggedValue = $"Endpoint disabled: {endpoint}";

            LoggingAssertions.CallExtensionsAndAssertLog(
                logger,
                DefaultEndpointRouterLoggingExtensions.EndpointDisabled,
                endpoint,
                LogLevel.Warning,
                new EventId(2_000_003),
                expectedLoggedValue);
        }
    }
}