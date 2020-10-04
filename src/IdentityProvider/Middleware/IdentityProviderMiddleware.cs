using System;
using System.Threading.Tasks;
using IdentityProvider.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IdentityProvider.Middleware
{
    public class IdentityProviderMiddleware
    {
        private readonly IEndpointRouter _endpointHandlers;
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;

        public IdentityProviderMiddleware(
            RequestDelegate next,
            IEndpointRouter endpointRouter,
            ILogger<IdentityProviderMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _endpointHandlers = endpointRouter ?? throw new ArgumentNullException(nameof(endpointRouter));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var handler = _endpointHandlers.Find(context);
            if (handler != null)
            {
                _logger.InvokingHandler(handler.Name, context.Request.Path.ToString());
                var result = await handler.HandleAsync(context, context.RequestAborted);
                _logger.ApplyingResult(result.Name);
                await result.ApplyAsync(context, context.RequestAborted);
                return;
            }

            await _next(context);
        }
    }
}