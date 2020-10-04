using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace IdentityProvider.Handlers
{
    public interface IEndpointHandler
    {
        public string Name { get; }

        public PathString Path { get; }

        public Task<IEndpointResult> HandleAsync(HttpContext context, CancellationToken cancellationToken = default);
    }
}