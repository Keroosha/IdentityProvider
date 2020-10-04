using IdentityProvider.Configuration.Options;
using IdentityProvider.Handlers;

namespace IdentityProvider.Extensions
{
    public static class EndpointOptionsExtensions
    {
        public static bool IsDefaultHandlerEnabled(
            this EndpointsOptions options,
            IEndpointHandler handler)
        {
            return handler.Name switch
            {
                Constants.EndpointNames.Authorize => options.EnableAuthorizeEndpoint,
                Constants.EndpointNames.CheckSession => options.EnableCheckSessionEndpoint,
                Constants.EndpointNames.DeviceAuthorization => options.EnableDeviceAuthorizationEndpoint,
                Constants.EndpointNames.Discovery => options.EnableDiscoveryEndpoint,
                Constants.EndpointNames.EndSession => options.EnableEndSessionEndpoint,
                Constants.EndpointNames.Introspection => options.EnableIntrospectionEndpoint,
                Constants.EndpointNames.Revocation => options.EnableTokenRevocationEndpoint,
                Constants.EndpointNames.Token => options.EnableTokenEndpoint,
                Constants.EndpointNames.UserInfo => options.EnableUserInfoEndpoint,
                _ => true
            };
        }
    }
}