using IdentityProvider.Hosting.Defaults;
using IdentityProvider.Tests.Unit.Common.Logging;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace IdentityProvider.Tests.Unit.Hosting.Defaults
{
    public class DefaultEndpointRouterLoggingExtensionsTests
    {
        [TestCase("/foo", "FooHandler")]
        [TestCase("/bar", "BarHandler")]
        public void PathMatchedToHandler_should_log_debug_with_eventId_2_000_000(string path, string handler)
        {
            var logger = new FakeLogger();
            var expectedLoggedValue = $"Request path {path} matched to endpoint handler: {handler}";

            LoggingAssertions.CallExtensionsAndAssertLog(
                logger,
                DefaultEndpointRouterLoggingExtensions.PathMatchedToHandler,
                path,
                handler,
                1,
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
                1,
                LogLevel.Trace,
                new EventId(2_000_001),
                expectedLoggedValue);
        }
    }
}