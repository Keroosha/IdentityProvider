﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IdentityProvider.Configuration.Options;
using IdentityProvider.Handlers;
using IdentityProvider.Hosting.Defaults;
using IdentityProvider.Tests.Unit.Common.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace IdentityProvider.Tests.Unit.Hosting.Defaults
{
    public class DefaultEndpointRouterTests
    {
        [Test]
        public void Find_should_return_null_for_incorrect_path()
        {
            var router = CreateRouter();
            var ctx = new DefaultHttpContext();
            ctx.Request.Path = new PathString("/incorrect");

            var handler = router.Find(ctx);

            Assert.Null(handler);
        }

        [TestCase("/ep1")]
        [TestCase("/ep2")]
        public void Find_should_find_path(string path)
        {
            var router = CreateRouter();
            var ctx = new DefaultHttpContext();
            ctx.Request.Path = new PathString(path);

            var handler = router.Find(ctx);

            Assert.NotNull(handler);
            Assert.IsInstanceOf<StubEndpointHandler>(handler);
        }

        [TestCase("/ep1/level1")]
        [TestCase("/ep2/level2/level2")]
        public void Find_should_not_find_nested_paths(string path)
        {
            var router = CreateRouter();
            var ctx = new DefaultHttpContext();
            ctx.Request.Path = new PathString(path);

            var handler = router.Find(ctx);

            Assert.Null(handler);
        }

        [TestCase("/ep1")]
        [TestCase("/ep2")]
        public void Find_should_find_first_registered_mapping(string path)
        {
            var options = new IdentityProviderOptions();
            var logger = new FakeLogger<DefaultEndpointRouter>();
            var handlers = new IEndpointHandler[]
            {
                new OtherStubEndpointHandler("ep1", new PathString("/ep1")),
                new StubEndpointHandler("ep1", new PathString("/ep1")),
                new OtherStubEndpointHandler("ep2", new PathString("/ep2")),
                new StubEndpointHandler("ep2", new PathString("/ep2"))
            };
            var router = new DefaultEndpointRouter(options, handlers, logger);
            var ctx = new DefaultHttpContext();
            ctx.Request.Path = new PathString(path);

            var handler = router.Find(ctx);

            Assert.NotNull(handler);
            Assert.IsInstanceOf<OtherStubEndpointHandler>(handler);
        }

        [TestCase("/ep1")]
        [TestCase("/ep2")]
        public void Find_should_return_null_for_disabled_handler(string path)
        {
            var options = new IdentityProviderOptions
            {
                Endpoints =
                {
                    EnableAuthorizeEndpoint = false,
                    EnableDiscoveryEndpoint = false
                }
            };
            var logger = new FakeLogger<DefaultEndpointRouter>();
            var handlers = new IEndpointHandler[]
            {
                new StubEndpointHandler(Constants.EndpointNames.Authorize, new PathString("/ep1")),
                new StubEndpointHandler(Constants.EndpointNames.Discovery, new PathString("/ep2")),
                new StubEndpointHandler("ep3", new PathString("/ep3"))
            };
            var router = new DefaultEndpointRouter(options, handlers, logger);
            var ctx = new DefaultHttpContext();
            ctx.Request.Path = new PathString(path);

            var handler = router.Find(ctx);

            Assert.Null(handler);
        }

        [TestCase("ep1", "/ep1")]
        [TestCase("ep2", "/ep2")]
        public void Find_should_logs_that_handler_found_and_enabled_when_path_matched(string handler, string path)
        {
            var options = new IdentityProviderOptions();
            var logger = new FakeLogger<DefaultEndpointRouter>();
            var handlers = new IEndpointHandler[]
            {
                new StubEndpointHandler("ep1", new PathString("/ep1")),
                new StubEndpointHandler("ep2", new PathString("/ep2")),
                new StubEndpointHandler("ep3", new PathString("/ep3"))
            };
            var router = new DefaultEndpointRouter(options, handlers, logger);
            var ctx = new DefaultHttpContext();
            ctx.Request.Path = new PathString(path);

            var _ = router.Find(ctx);

            LoggingAssertions.AssertLog(
                logger,
                0,
                2,
                LogLevel.Debug,
                new EventId(2_000_000),
                $"Request path {path} matched to endpoint handler: {handler}");

            LoggingAssertions.AssertLog(
                logger,
                1,
                2,
                LogLevel.Debug,
                new EventId(2_000_002),
                $"Handler enabled: {handler}");
        }


        [TestCase("ep1", "/ep1")]
        [TestCase("ep2", "/ep2")]
        public void Find_should_logs_warning_if_handler_found_but_disabled(string handler, string path)
        {
            var options = new IdentityProviderOptions();
            var logger = new FakeLogger<DefaultEndpointRouter>();
            var handlers = new IEndpointHandler[]
            {
                new StubEndpointHandler("ep1", new PathString("/ep1")),
                new StubEndpointHandler("ep2", new PathString("/ep2")),
                new StubEndpointHandler("ep3", new PathString("/ep3"))
            };
            var router = new DefaultEndpointRouter(options, handlers, logger);
            var ctx = new DefaultHttpContext();
            ctx.Request.Path = new PathString(path);

            var _ = router.Find(ctx);

            LoggingAssertions.AssertLog(
                logger,
                0,
                2,
                LogLevel.Debug,
                new EventId(2_000_000),
                $"Request path {path} matched to endpoint handler: {handler}");

            LoggingAssertions.AssertLog(
                logger,
                1,
                2,
                LogLevel.Debug,
                new EventId(2_000_002),
                $"Handler enabled: {handler}");
        }

        [TestCase("ep5", "/ep5")]
        [TestCase("ep6", "/ep6")]
        public void Find_should_logs_that_nothing_was_found_when_path_not_matched(string handler, string path)
        {
            var options = new IdentityProviderOptions();
            var logger = new FakeLogger<DefaultEndpointRouter>();
            var handlers = new IEndpointHandler[]
            {
                new StubEndpointHandler("ep1", new PathString("/ep1")),
                new StubEndpointHandler("ep2", new PathString("/ep2")),
                new StubEndpointHandler("ep3", new PathString("/ep3"))
            };
            var router = new DefaultEndpointRouter(options, handlers, logger);
            var ctx = new DefaultHttpContext();
            ctx.Request.Path = new PathString(path);
            var expectedLoggedValue = $"No endpoint entry found for request path: {path}";

            var _ = router.Find(ctx);

            LoggingAssertions.AssertLog(
                logger,
                0,
                1,
                LogLevel.Trace,
                new EventId(2_000_001),
                expectedLoggedValue);
        }

        [TestCaseSource(nameof(IncorrectCtorArguments))]
        public void Ctor_DefaultEndpointRouter_should_throws_with_arguments_are_null(
            IdentityProviderOptions options,
            IEnumerable<IEndpointHandler> endpointHandlers,
            ILogger<DefaultEndpointRouter> logger)
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var _ = new DefaultEndpointRouter(options, endpointHandlers, logger);
            });
        }

        public static object[][] IncorrectCtorArguments()
        {
            return new[]
            {
                new object[]
                {
                    new IdentityProviderOptions(),
                    null!,
                    null!
                },
                new object[]
                {
                    null!,
                    new IEndpointHandler[] { },
                    null!
                },
                new object[]
                {
                    null!,
                    null!,
                    new FakeLogger<DefaultEndpointRouter>()
                },
                new object[]
                {
                    null!,
                    null!,
                    null!
                }
            };
        }

        private static DefaultEndpointRouter CreateRouter()
        {
            var options = new IdentityProviderOptions();
            var logger = new FakeLogger<DefaultEndpointRouter>();
            var handlers = new IEndpointHandler[]
            {
                new StubEndpointHandler("ep1", new PathString("/ep1")),
                new StubEndpointHandler("ep2", new PathString("/ep2")),
                new StubEndpointHandler("ep3", new PathString("/ep3"))
            };
            var router = new DefaultEndpointRouter(options, handlers, logger);
            return router;
        }

        private class StubEndpointHandler : IEndpointHandler
        {
            public StubEndpointHandler(string name, PathString path)
            {
                Name = name;
                Path = path;
            }

            public string Name { get; }
            public PathString Path { get; }

            public Task<IEndpointResult> HandleAsync(HttpContext context, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }
        }

        private class OtherStubEndpointHandler : IEndpointHandler
        {
            public OtherStubEndpointHandler(string name, PathString path)
            {
                Name = name;
                Path = path;
            }

            public string Name { get; }
            public PathString Path { get; }

            public Task<IEndpointResult> HandleAsync(HttpContext context, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }
        }
    }
}