using IdentityProvider.Endpoints;
using Microsoft.AspNetCore.Http;

namespace IdentityProvider.Hosting
{
    public interface IEndpointRouter
    {
        public IEndpoint? Find(HttpContext context);
    }
}