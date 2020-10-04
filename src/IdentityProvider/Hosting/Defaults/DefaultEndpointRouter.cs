using System;
using System.Collections.Generic;
using System.Linq;
using IdentityProvider.Configuration.Options;
using IdentityProvider.Endpoints;
using IdentityProvider.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IdentityProvider.Hosting.Defaults
{
    public class DefaultEndpointRouter : IEndpointRouter
    {
        private readonly IEndpoint[] _endpoints;
        private readonly ILogger _logger;
        private readonly EndpointsOptions _options;

        public DefaultEndpointRouter(
            IdentityProviderOptions options,
            IEnumerable<IEndpoint> endpoints,
            ILogger<DefaultEndpointRouter> logger)
        {
            if (endpoints == null)
            {
                throw new ArgumentNullException(nameof(endpoints));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _options = options.Endpoints;
            _endpoints = endpoints.ToArray();
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IEndpoint? Find(HttpContext context)
        {
            var requestPath = context.Request.Path;
            var requestPathString = requestPath.ToString();
            foreach (var endpoint in _endpoints)
            {
                if (requestPath.Equals(endpoint.Path, StringComparison.Ordinal))
                {
                    _logger.PathMatchedToEndpoint(requestPathString, endpoint.Name);
                    if (_options.IsEndpointEnabled(endpoint))
                    {
                        _logger.EndpointEnabled(endpoint.Name);
                        return endpoint;
                    }

                    _logger.EndpointDisabled(endpoint.Name);
                    return null;
                }
            }

            _logger.NoEndpointForPath(requestPathString);
            return null;
        }
    }
}