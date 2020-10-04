using System;
using System.Threading.Tasks;
using IdentityProvider.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IdentityProvider.Middleware
{
    public class IdentityProviderMiddleware
    {
        private readonly IEndpointRouter _endpointRouter;
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;

        public IdentityProviderMiddleware(
            RequestDelegate next,
            IEndpointRouter endpointRouter,
            ILogger<IdentityProviderMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _endpointRouter = endpointRouter ?? throw new ArgumentNullException(nameof(endpointRouter));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = _endpointRouter.Find(context);
            if (endpoint != null)
            {
                _logger.InvokingEndpoint(endpoint.Name, context.Request.Path.ToString());
                var result = await endpoint.ProcessAsync(context, context.RequestAborted);
                _logger.ExecutingResult(result.Name);
                await result.ExecuteAsync(context, context.RequestAborted);
                return;
            }

            await _next(context);
        }
    }
}