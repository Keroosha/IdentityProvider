using System.Diagnostics.CodeAnalysis;

namespace IdentityProvider.Configuration.Options
{
    /// <summary>
    ///     The <see cref="IdentityProviderOptions" /> class is the top level container for all configuration settings of
    ///     IdentityProvider.
    /// </summary>
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
    public class IdentityProviderOptions
    {
        /// <summary>
        ///     Gets or sets the endpoint configuration.
        /// </summary>
        /// <value>
        ///     The endpoints configuration.
        /// </value>
        public EndpointsOptions Endpoints { get; set; } = new EndpointsOptions();
    }
}