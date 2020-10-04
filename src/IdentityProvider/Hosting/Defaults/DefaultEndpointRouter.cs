using System;
using System.Collections.Generic;
using System.Linq;
using IdentityProvider.Configuration.Options;
using IdentityProvider.Extensions;
using IdentityProvider.Handlers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IdentityProvider.Hosting.Defaults
{
    public class DefaultEndpointRouter : IEndpointRouter
    {
        private readonly IEndpointHandler[] _endpointHandlers;
        private readonly ILogger _logger;
        private readonly EndpointsOptions _options;

        public DefaultEndpointRouter(
            IdentityProviderOptions options,
            IEnumerable<IEndpointHandler> endpointHandlers,
            ILogger<DefaultEndpointRouter> logger)
        {
            if (endpointHandlers == null)
            {
                throw new ArgumentNullException(nameof(endpointHandlers));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _options = options.Endpoints;
            _endpointHandlers = endpointHandlers.ToArray();
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IEndpointHandler? Find(HttpContext context)
        {
            var requestPath = context.Request.Path;
            var requestPathString = requestPath.ToString();
            foreach (var endpointHandler in _endpointHandlers)
            {
                if (requestPath.Equals(endpointHandler.Path, StringComparison.Ordinal))
                {
                    _logger.PathMatchedToHandler(requestPathString, endpointHandler.Name);
                    if (_options.IsDefaultHandlerEnabled(endpointHandler))
                    {
                        _logger.HandlerEnabled(endpointHandler.Name);
                        return endpointHandler;
                    }

                    _logger.HandlerDisabled(endpointHandler.Name);
                    return null;
                }
            }

            _logger.NoEndpointForPath(requestPathString);

            return null;
        }
    }
}