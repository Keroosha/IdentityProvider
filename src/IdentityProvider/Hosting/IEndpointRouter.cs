using IdentityProvider.Handlers;
using Microsoft.AspNetCore.Http;

namespace IdentityProvider.Hosting
{
    public interface IEndpointRouter
    {
        public IEndpointHandler? Find(HttpContext context);
    }
}